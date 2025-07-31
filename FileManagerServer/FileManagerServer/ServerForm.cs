using System;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.Logging;

namespace FileManagerServer
{
    public partial class ServerForm : Form
    {
        private TcpListener tcpListener;
        private Thread tcpListenerThread;
        private bool isServerRunning = false;
        private List<TcpClient> connectedClients = new List<TcpClient>();
        private Dictionary<TcpClient, string> clientUsernames = new Dictionary<TcpClient, string>();
        private string serverPath;
        private int maxClients;
        private int bufferSize;
        private readonly object clientLock = new(); // để thread-safe

        // UI Controls
        private TextBox txtServerPath;
        private TextBox txtPort;
        private TextBox txtMaxClients;
        private Button btnStart;
        private Button btnStop;
        private Label lblStatus;
        private ListView lstClients;
        private TextBox txtLog;
        private ToolStripStatusLabel toolStripStatusLabel;

        public ServerForm()
        {
            LoadConfiguration();
            InitializeComponent();
        }

        private void LoadConfiguration()
        {
            serverPath = ConfigurationManager.AppSettings["DefaultServerPath"] ?? "C:\\FileManagerServer";
            maxClients = int.Parse(ConfigurationManager.AppSettings["MaxClients"] ?? "10");
            bufferSize = int.Parse(ConfigurationManager.AppSettings["BufferSize"] ?? "8192");

            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(900, 650);
            this.Text = "File Manager Server v1.0";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 550);

            CreateControls();
            SetupTimer();
        }

        private void CreateControls()
        {
            // Menu Strip
            var menuStrip = new MenuStrip();
            var serverMenu = new ToolStripMenuItem("Server");
            var viewMenu = new ToolStripMenuItem("View");
            var helpMenu = new ToolStripMenuItem("Help");

            var startItem = new ToolStripMenuItem("Start Server") { ShortcutKeys = Keys.F5 };
            var stopItem = new ToolStripMenuItem("Stop Server") { ShortcutKeys = Keys.F6, Enabled = false };
            var exitItem = new ToolStripMenuItem("Exit") { ShortcutKeys = Keys.Alt | Keys.F4 };

            var clearLogItem = new ToolStripMenuItem("Clear Log") { ShortcutKeys = Keys.Control | Keys.L };
            var settingsItem = new ToolStripMenuItem("Settings");
            var aboutItem = new ToolStripMenuItem("About");

            serverMenu.DropDownItems.AddRange(new ToolStripItem[] { startItem, stopItem, new ToolStripSeparator(), exitItem });
            viewMenu.DropDownItems.AddRange(new ToolStripItem[] { clearLogItem, settingsItem });
            helpMenu.DropDownItems.Add(aboutItem);

            menuStrip.Items.AddRange(new ToolStripItem[] { serverMenu, viewMenu, helpMenu });

            // Status Strip
            var statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel("Server Stopped");
            var toolStripStatusClients = new ToolStripStatusLabel("Clients: 0");
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripStatusClients });

            // Server Configuration Group
            var groupBoxConfig = new GroupBox
            {
                Text = "Server Configuration",
                Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold),
                Location = new Point(12, 30),
                Size = new Size(860, 100)
            };

            txtServerPath = new TextBox
            {
                Location = new Point(100, 25),
                Size = new Size(300, 20),
                Text = serverPath
            };

            var btnBrowse = new Button
            {
                Text = "Browse...",
                Location = new Point(410, 23),
                Size = new Size(70, 25)
            };

            txtPort = new TextBox
            {
                Location = new Point(550, 25),
                Size = new Size(60, 20),
                Text = "8080"
            };

            txtMaxClients = new TextBox
            {
                Location = new Point(750, 25),
                Size = new Size(60, 20),
                Text = maxClients.ToString()
            };

            btnStart = new Button
            {
                Text = "Start Server",
                Location = new Point(100, 60),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnStop = new Button
            {
                Text = "Stop Server",
                Location = new Point(210, 60),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };

            lblStatus = new Label
            {
                Text = "Server Stopped",
                Location = new Point(330, 67),
                Size = new Size(100, 20),
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold),
                ForeColor = Color.Red
            };

            groupBoxConfig.Controls.AddRange(new Control[] {
                new Label { Text = "Server Path:", Location = new Point(15, 28), Size = new Size(80, 20) },
                txtServerPath, btnBrowse,
                new Label { Text = "Port:", Location = new Point(500, 28), Size = new Size(35, 20) },
                txtPort,
                new Label { Text = "Max Clients:", Location = new Point(670, 28), Size = new Size(75, 20) },
                txtMaxClients,
                btnStart, btnStop, lblStatus
            });

            // Tab Control
            var tabControl = new TabControl
            {
                Location = new Point(12, 140),
                Size = new Size(860, 450)
            };

            // Clients Tab
            var tabPageClients = new TabPage("👥 Connected Clients");
            CreateClientsTab(tabPageClients);

            // Server Log Tab
            var tabPageLog = new TabPage("📋 Server Log");
            CreateLogTab(tabPageLog);

            tabControl.TabPages.Add(tabPageClients);
            tabControl.TabPages.Add(tabPageLog);

            // Event handlers
            startItem.Click += (s, e) => StartServer();
            stopItem.Click += (s, e) => StopServer();
            btnStart.Click += (s, e) => StartServer();
            btnStop.Click += (s, e) => StopServer();
            btnBrowse.Click += BtnBrowse_Click;
            clearLogItem.Click += (s, e) => txtLog.Clear();
            settingsItem.Click += SettingsItem_Click;
            aboutItem.Click += AboutItem_Click;
            exitItem.Click += (s, e) => this.Close();

            // Store references for menu items
            this.Tag = new { startItem, stopItem, toolStripStatusClients };

            // Add all controls to form
            this.Controls.AddRange(new Control[] {
                menuStrip, groupBoxConfig, tabControl, statusStrip
            });
        }

        private void CreateClientsTab(TabPage tabPage)
        {
            var groupBoxClients = new GroupBox
            {
                Text = "Connected Clients",
                Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(830, 350)
            };

            lstClients = new ListView
            {
                Location = new Point(10, 25),
                Size = new Size(810, 315),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            lstClients.Columns.Add("Username", 150);
            lstClients.Columns.Add("IP Address", 120);
            lstClients.Columns.Add("Connected Time", 150);
            lstClients.Columns.Add("Status", 100);
            lstClients.Columns.Add("Last Activity", 150);

            var btnDisconnectClient = new Button
            {
                Text = "Disconnect Selected",
                Location = new Point(10, 380),
                Size = new Size(130, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            var btnBroadcastMessage = new Button
            {
                Text = "Broadcast Message",
                Location = new Point(150, 380),
                Size = new Size(130, 30),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnDisconnectClient.Click += BtnDisconnectClient_Click;
            btnBroadcastMessage.Click += BtnBroadcastMessage_Click;

            groupBoxClients.Controls.Add(lstClients);
            tabPage.Controls.AddRange(new Control[] { groupBoxClients, btnDisconnectClient, btnBroadcastMessage });
        }

        private void CreateLogTab(TabPage tabPage)
        {
            var groupBoxLog = new GroupBox
            {
                Text = "Server Activity Log",
                Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(830, 380)
            };

            txtLog = new TextBox
            {
                Location = new Point(10, 25),
                Size = new Size(810, 345),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Consolas", 8.25F),
                BackColor = Color.Black,
                ForeColor = Color.Lime
            };

            var btnClearLog = new Button
            {
                Text = "Clear Log",
                Location = new Point(10, 400),
                Size = new Size(80, 25)
            };

            var btnSaveLog = new Button
            {
                Text = "Save Log",
                Location = new Point(100, 400),
                Size = new Size(80, 25)
            };

            btnClearLog.Click += (s, e) => txtLog.Clear();
            btnSaveLog.Click += BtnSaveLog_Click;

            groupBoxLog.Controls.Add(txtLog);
            tabPage.Controls.AddRange(new Control[] { groupBoxLog, btnClearLog, btnSaveLog });
        }

        private void SetupTimer()
        {
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = isServerRunning ? "Server Running" : "Server Stopped";
            var controls = (dynamic)this.Tag;
            controls.toolStripStatusClients.Text = $"Clients: {connectedClients.Count}";
        }

        private void StartServer()
        {
            try
            {
                int port = int.Parse(txtPort.Text);
                maxClients = int.Parse(txtMaxClients.Text);

                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListenerThread = new Thread(new ThreadStart(ListenForClients));
                tcpListenerThread.Start();

                isServerRunning = true;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                lblStatus.Text = "Server Running";
                lblStatus.ForeColor = Color.Green;

                var controls = (dynamic)this.Tag;
                controls.startItem.Enabled = false;
                controls.stopItem.Enabled = true;

                LogMessage($"Server started on port {port}");
                LogMessage($"Server path: {serverPath}");
                LogMessage($"Max clients: {maxClients}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            lock (clientLock)
            {
                foreach (var client in connectedClients)
                {
                    SendFileListToClient(client, serverPath);
                }
            }
        }


        private void SendFileListToClient(TcpClient client, string relativePath)
        {
            try
            {
                string fullPath = GetFullPath(relativePath);

                if (!Directory.Exists(fullPath))
                {
                    SendResponse(client, "LIST_FILES", "[]");
                    return;
                }

                var entries = new List<Dictionary<string, object>>();

                // Thêm thư mục con
                foreach (var dir in Directory.GetDirectories(fullPath))
                {
                    var dirInfo = new DirectoryInfo(dir);
                    entries.Add(new Dictionary<string, object>
                    {
                        ["Name"] = dirInfo.Name,
                        ["Type"] = "folder",
                        ["LastModified"] = dirInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["Path"] = GetRelativePath(dir, serverPath) // Trả path tương đối so với thư mục gốc
                    });
                }

                // Thêm file
                foreach (var file in Directory.GetFiles(fullPath))
                {
                    var fileInfo = new FileInfo(file);
                    entries.Add(new Dictionary<string, object>
                    {
                        ["Name"] = fileInfo.Name,
                        ["Type"] = "file",
                        ["Size"] = fileInfo.Length,
                        ["LastModified"] = fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["Path"] = GetRelativePath(file, serverPath)
                    });
                }

                string json = JsonSerializer.Serialize(entries);
                SendResponse(client, "LIST_FILES", json);
            }
            catch (Exception ex)
            {
                SendResponse(client, "LIST_FILES", "[]");
                //Log?.Invoke($"[ERROR] SendFileListToClient: {ex.Message}");
            }
        }


        private void ShowDebug(string message)
        {
            // Dùng form chính gọi MessageBox từ UI thread
            if (Application.OpenForms.Count > 0)
            {
                var mainForm = Application.OpenForms[0];
                mainForm.Invoke((MethodInvoker)(() =>
                {
                    MessageBox.Show(message, "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
        }



        private string GetRelativePath(string fullPath, string basePath)
        {
            string relative = fullPath.Replace(basePath, "").Replace("\\", "/");
            return string.IsNullOrEmpty(relative) || relative == "/" ? "/" : relative;
        }




        private void StopServer()
        {
            isServerRunning = false;

            tcpListener?.Stop(); // Dừng nhận kết nối mới

            // Đợi luồng lắng nghe thoát (nếu còn đang chạy)
            if (tcpListenerThread != null && tcpListenerThread.IsAlive)
            {
                tcpListenerThread.Join(); // Đợi luồng kết thúc an toàn
            }

            // Ngắt kết nối tất cả client
            foreach (var client in connectedClients.ToList())
            {
                client.Close();
            }
            connectedClients.Clear();
            clientUsernames.Clear();

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            lblStatus.Text = "Server Stopped";
            lblStatus.ForeColor = Color.Red;

            var controls = (dynamic)this.Tag;
            controls.startItem.Enabled = true;
            controls.stopItem.Enabled = false;

            this.Invoke((MethodInvoker)delegate
            {
                lstClients.Items.Clear();
            });

            LogMessage("Server stopped");
        }

        private void ListenForClients()
        {
            tcpListener.Start();

            while (isServerRunning)
            {
                try
                {
                    if (connectedClients.Count >= maxClients)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    TcpClient client = tcpListener.AcceptTcpClient();
                    connectedClients.Add(client);

                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);

                    LogMessage($"Client connected: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");
                }
                catch (Exception ex)
                {
                    if (isServerRunning)
                    {
                        LogMessage($"Error accepting client: {ex.Message}");
                    }
                }
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            byte[] message = new byte[bufferSize];
            int bytesRead;
            string clientIP = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, bufferSize);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    break;
                }

                string jsonMessage = Encoding.UTF8.GetString(message, 0, bytesRead);
                ProcessClientMessage(tcpClient, jsonMessage);
            }

            // Client disconnected
            connectedClients.Remove(tcpClient);
            if (clientUsernames.ContainsKey(tcpClient))
            {
                string username = clientUsernames[tcpClient];
                clientUsernames.Remove(tcpClient);
                LogMessage($"Client disconnected: {username} ({clientIP})");

                this.Invoke((MethodInvoker)delegate
                {
                    UpdateClientsList();
                });
            }
            tcpClient.Close();
        }

        private void ProcessClientMessage(TcpClient client, string jsonMessage)
        {
            try
            {
                var request = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonMessage);
                string type = request["Type"].ToString();
                string data = request.ContainsKey("Data") ? request["Data"].ToString() : "";

                switch (type)
                {
                    case "LOGIN":
                        HandleLogin(client, data);
                        break;
                    case "LIST_FILES":
                        HandleListFiles(client, data);
                        break;
                    case "DOWNLOAD_FILE":
                        HandleDownloadFile(client, data);
                        break;
                    case "UPLOAD_FILE":
                        HandleUploadFile(client, data, request.ContainsKey("FileData") ? request["FileData"].ToString() : "");
                        break;
                    case "DELETE_FILE":
                        HandleDeleteFile(client, data);
                        break;
                    case "CHAT_MESSAGE":
                        HandleChatMessage(client, data);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error processing message: {ex.Message}");
                SendResponse(client, "ERROR", ex.Message);
            }
        }

        private void HandleLogin(TcpClient client, string username)
        {
            clientUsernames[client] = username;
            string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            LogMessage($"User logged in: {username} ({clientIP})");

            this.Invoke((MethodInvoker)delegate
            {
                UpdateClientsList();
            });

            SendResponse(client, "LOGIN", "Login successful");
        }

        private void HandleListFiles(TcpClient client, string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    SendResponse(client, "LIST_FILES", "Folder not found", "ERROR");
                    return;
                }

                var entries = new List<Dictionary<string, object>>();

                // Add folders
                foreach (string dir in Directory.GetDirectories(folderPath))
                {
                    var info = new DirectoryInfo(dir);
                    entries.Add(new Dictionary<string, object>
                    {
                        ["Name"] = info.Name,
                        ["Type"] = "folder",
                        ["Size"] = 0,
                        ["LastModified"] = info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["Path"] = dir
                    });
                }

                // Add files
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    var info = new FileInfo(file);
                    entries.Add(new Dictionary<string, object>
                    {
                        ["Name"] = info.Name,
                        ["Type"] = "file",
                        ["Size"] = info.Length,
                        ["LastModified"] = info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["Path"] = file
                    });
                }

                SendResponse(client, "LIST_FILES", entries); // Status = OK by default
            }
            catch (Exception ex)
            {
                SendResponse(client, "LIST_FILES", ex.Message, "ERROR");
            }
        }


        private void HandleDownloadFile(TcpClient client, string filePath)
        {
            try
            {
                // Đảm bảo filePath là đường dẫn tương đối
                string relativePath = filePath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

                // Ghép với thư mục gốc server
                string fullPath = Path.Combine(serverPath, relativePath);

                LogMessage($"Download requested: {relativePath}");
                LogMessage($"Full path resolved: {fullPath}");

                if (!File.Exists(fullPath))
                {
                    SendResponse(client, "DOWNLOAD_FILE", "File not found", "ERROR");
                    return;
                }

                byte[] fileData = File.ReadAllBytes(fullPath);
                string base64Data = Convert.ToBase64String(fileData);

                var response = new Dictionary<string, object>
                {
                    ["Type"] = "DOWNLOAD_FILE",
                    ["Status"] = "OK",
                    ["Data"] = filePath,         // Trả lại đường dẫn gốc client yêu cầu
                    ["FileData"] = base64Data
                };

                SendJsonResponse(client, response);
                LogMessage($"File downloaded: {relativePath} by {GetClientUsername(client)}");
            }
            catch (Exception ex)
            {
                SendResponse(client, "DOWNLOAD_FILE", ex.Message, "ERROR");
            }
        }


        private void HandleUploadFile(TcpClient client, string filePath, string base64Data)
        {
            try
            {
                string fullPath = Path.Combine(serverPath, filePath.TrimStart('/').Replace('/', '\\'));
                string directory = Path.GetDirectoryName(fullPath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                byte[] fileData = Convert.FromBase64String(base64Data);
                File.WriteAllBytes(fullPath, fileData);

                SendResponse(client, "UPLOAD_FILE", "File uploaded successfully");
                LogMessage($"File uploaded: {filePath} by {GetClientUsername(client)}");
            }
            catch (Exception ex)
            {
                SendResponse(client, "UPLOAD_FILE", ex.Message, "ERROR");
            }
        }

        private void HandleDeleteFile(TcpClient client, string filePath)
        {
            try
            {
                string fullPath = Path.Combine(serverPath, filePath.TrimStart('/').Replace('/', '\\'));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    SendResponse(client, "DELETE_FILE", "File deleted successfully");
                    LogMessage($"File deleted: {filePath} by {GetClientUsername(client)}");
                }
                else if (Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath, true);
                    SendResponse(client, "DELETE_FILE", "Directory deleted successfully");
                    LogMessage($"Directory deleted: {filePath} by {GetClientUsername(client)}");
                }
                else
                {
                    SendResponse(client, "DELETE_FILE", "File or directory not found", "ERROR");
                }
            }
            catch (Exception ex)
            {
                SendResponse(client, "DELETE_FILE", ex.Message, "ERROR");
            }
        }

        private void HandleChatMessage(TcpClient client, string message)
        {
            string username = GetClientUsername(client);
            string chatMessage = $"{username}: {message}";

            // Broadcast to all clients
            foreach (var connectedClient in connectedClients.ToList())
            {
                if (connectedClient != client)
                {
                    SendResponse(connectedClient, "CHAT_MESSAGE", chatMessage);
                }
            }

            LogMessage($"Chat message from {username}: {message}");
        }

        private void SendResponse(TcpClient client, string type, object data, string status = "OK")
        {
            var response = new Dictionary<string, object>
            {
                ["Type"] = type,
                ["Status"] = status,
                ["Data"] = data
            };

            string json = JsonSerializer.Serialize(response) + "\n";
            var stream = client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            stream.Write(buffer, 0, buffer.Length);
        }


        private void SendJsonResponse(TcpClient client, Dictionary<string, object> response)
        {
            try
            {
                string jsonResponse = JsonSerializer.Serialize(response);
                byte[] responseBytes = Encoding.UTF8.GetBytes(jsonResponse);
                NetworkStream stream = client.GetStream();
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
            catch (Exception ex)
            {
                LogMessage($"Error sending response: {ex.Message}");
            }
        }

        private string GetClientUsername(TcpClient client)
        {
            return clientUsernames.ContainsKey(client) ? clientUsernames[client] : "Unknown";
        }

        private void UpdateClientsList()
        {
            lstClients.Items.Clear();
            foreach (var client in connectedClients)
            {
                string username = GetClientUsername(client);
                string ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

                var item = new ListViewItem(username);
                item.SubItems.Add(ip);
                item.SubItems.Add(DateTime.Now.ToString("HH:mm:ss"));
                item.SubItems.Add("Connected");
                item.SubItems.Add(DateTime.Now.ToString("HH:mm:ss"));
                item.Tag = client;

                lstClients.Items.Add(item);
            }
        }

        private void LogMessage(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

            this.Invoke((MethodInvoker)delegate
            {
                txtLog.AppendText(logEntry + "\r\n");
                txtLog.ScrollToCaret();
            });
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtServerPath.Text = dialog.SelectedPath;
                    serverPath = dialog.SelectedPath; // ← Thêm dòng này
                }
            }
        }


        private void BtnDisconnectClient_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count > 0)
            {
                var client = (TcpClient)lstClients.SelectedItems[0].Tag;
                client.Close();
                connectedClients.Remove(client);
                if (clientUsernames.ContainsKey(client))
                {
                    clientUsernames.Remove(client);
                }
                UpdateClientsList();
            }
        }

        private void BtnBroadcastMessage_Click(object sender, EventArgs e)
        {
            string message = Microsoft.VisualBasic.Interaction.InputBox("Enter message to broadcast:", "Broadcast Message", "");
            if (!string.IsNullOrEmpty(message))
            {
                foreach (var client in connectedClients.ToList())
                {
                    SendResponse(client, "CHAT_MESSAGE", $"Server: {message}");
                }
                LogMessage($"Broadcast message sent: {message}");
            }
        }

        private void BtnSaveLog_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                dialog.FileName = $"ServerLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dialog.FileName, txtLog.Text);
                    MessageBox.Show("Log saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void SettingsItem_Click(object sender, EventArgs e)
        {
            string settings = $"Current Server Settings:\n\n" +
                            $"Server Path: {serverPath}\n" +
                            $"Port: {txtPort.Text}\n" +
                            $"Max Clients: {maxClients}\n" +
                            $"Buffer Size: {bufferSize} bytes\n" +
                            $"Connected Clients: {connectedClients.Count}";

            MessageBox.Show(settings, "Server Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("File Manager Server v1.0\n\nA simple file sharing server with chat functionality.\n\nUsing System.Text.Json for improved performance.",
                           "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GetFullPath(string relativePath)
        {
            if (Path.IsPathRooted(relativePath))
            {
                // Nếu path gửi từ client là tuyệt đối, bỏ qua – tránh rủi ro bảo mật
                return serverPath;
            }

            string combined = Path.Combine(serverPath, relativePath.TrimStart('/', '\\'));
            return Path.GetFullPath(combined); // chuẩn hóa, loại bỏ ".."
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isServerRunning)
            {
                StopServer();
            }
            base.OnFormClosing(e);
        }
    }
}
