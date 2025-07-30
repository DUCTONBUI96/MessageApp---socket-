using System;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace FileManagerClient
{
    public partial class ClientForm : Form
    {
        private TcpClient tcpClient;
        private NetworkStream serverStream;
        private Thread clientReceiveThread;
        private bool isConnected = false;
        private string currentPath = "/";
        private string defaultDownloadPath;

        // UI Controls
        private ProgressBar progressBar;
        private ToolStripStatusLabel toolStripStatusLabel;

        public ClientForm()
        {
            LoadConfiguration();
            //InitializeComponent();
            this.Size = new Size(1000, 700);
            this.Text = "File Manager Client v1.0";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(900, 600);
            CreateControls();
            SetupTimer();
        }

        private void LoadConfiguration()
        {
            defaultDownloadPath = ConfigurationManager.AppSettings["DownloadPath"] ?? "Downloads";
            if (!Path.IsPathRooted(defaultDownloadPath))
            {
                defaultDownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), defaultDownloadPath);
            }
            if (!Directory.Exists(defaultDownloadPath))
            {
                Directory.CreateDirectory(defaultDownloadPath);
            }
        }

        private void CreateControls()
        {
            // Menu Strip
            var menuStrip = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("File");
            var editMenu = new ToolStripMenuItem("Edit");
            var viewMenu = new ToolStripMenuItem("View");
            var helpMenu = new ToolStripMenuItem("Help");

            var connectItem = new ToolStripMenuItem("Connect") { ShortcutKeys = Keys.F5 };
            var disconnectItem = new ToolStripMenuItem("Disconnect") { ShortcutKeys = Keys.F6, Enabled = false };
            var exitItem = new ToolStripMenuItem("Exit") { ShortcutKeys = Keys.Alt | Keys.F4 };

            var uploadItem = new ToolStripMenuItem("Upload File") { ShortcutKeys = Keys.Control | Keys.U };
            var downloadItem = new ToolStripMenuItem("Download File") { ShortcutKeys = Keys.Control | Keys.D };
            var deleteItem = new ToolStripMenuItem("Delete File") { ShortcutKeys = Keys.Delete };

            var refreshItem = new ToolStripMenuItem("Refresh") { ShortcutKeys = Keys.F5 };
            var aboutItem = new ToolStripMenuItem("About");

            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { connectItem, disconnectItem, new ToolStripSeparator(), exitItem });
            editMenu.DropDownItems.AddRange(new ToolStripItem[] { uploadItem, downloadItem, deleteItem });
            viewMenu.DropDownItems.Add(refreshItem);
            helpMenu.DropDownItems.Add(aboutItem);

            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, editMenu, viewMenu, helpMenu });

            // Status Strip
            var statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel("Ready");
            var toolStripStatusConnection = new ToolStripStatusLabel("Disconnected");
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripStatusConnection });

            // Connection Group
            var groupBoxConnection = new GroupBox
            {
                Text = "Server Connection",
                Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold),
                Location = new Point(12, 30),
                Size = new Size(960, 80)
            };

            var txtServerIP = new TextBox
            {
                Location = new Point(80, 30),
                Size = new Size(120, 20),
                Text = ConfigurationManager.AppSettings["DefaultServerIP"] ?? "127.0.0.1"
            };

            var txtPort = new TextBox
            {
                Location = new Point(250, 30),
                Size = new Size(60, 20),
                Text = ConfigurationManager.AppSettings["DefaultPort"] ?? "8080"
            };

            var txtUsername = new TextBox
            {
                Location = new Point(390, 30),
                Size = new Size(100, 20),
                Text = ConfigurationManager.AppSettings["DefaultUsername"] ?? "User1"
            };

            var btnConnect = new Button
            {
                Text = "Connect",
                Location = new Point(500, 25),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            var btnDisconnect = new Button
            {
                Text = "Disconnect",
                Location = new Point(590, 25),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };

            lblStatus = new Label
            {
                Text = "Disconnected",
                Location = new Point(700, 33),
                Size = new Size(100, 20),
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold),
                ForeColor = Color.Red
            };

            // Event handlers for connection
            connectItem.Click += (s, e) => ConnectToServer(txtServerIP.Text, int.Parse(txtPort.Text), txtUsername.Text, btnConnect, btnDisconnect, connectItem, disconnectItem);
            disconnectItem.Click += (s, e) => Disconnect(btnConnect, btnDisconnect, connectItem, disconnectItem);
            btnConnect.Click += (s, e) => ConnectToServer(txtServerIP.Text, int.Parse(txtPort.Text), txtUsername.Text, btnConnect, btnDisconnect, connectItem, disconnectItem);
            btnDisconnect.Click += (s, e) => Disconnect(btnConnect, btnDisconnect, connectItem, disconnectItem);
            exitItem.Click += (s, e) => this.Close();
            aboutItem.Click += AboutToolStripMenuItem_Click;

            groupBoxConnection.Controls.AddRange(new Control[] {
                new Label { Text = "Server IP:", Location = new Point(15, 33), Size = new Size(60, 20) },
                txtServerIP,
                new Label { Text = "Port:", Location = new Point(210, 33), Size = new Size(35, 20) },
                txtPort,
                new Label { Text = "Username:", Location = new Point(320, 33), Size = new Size(65, 20) },
                txtUsername,
                btnConnect, btnDisconnect, lblStatus
            });

            // Tab Control
            var tabControl = new TabControl
            {
                Location = new Point(12, 120),
                Size = new Size(960, 520)
            };

            // File Manager Tab
            var tabPageFiles = new TabPage("📁 File Manager");
            CreateFileManagerTab(tabPageFiles, uploadItem, downloadItem, deleteItem, refreshItem);

            // Chat Tab
            var tabPageChat = new TabPage("💬 Chat");
            CreateChatTab(tabPageChat);

            tabControl.TabPages.Add(tabPageFiles);
            tabControl.TabPages.Add(tabPageChat);

            // Add all controls to form
            this.Controls.AddRange(new Control[] {
                menuStrip, groupBoxConnection, tabControl, statusStrip
            });
        }

        private void CreateFileManagerTab(TabPage tabPage, ToolStripMenuItem uploadItem, ToolStripMenuItem downloadItem, ToolStripMenuItem deleteItem, ToolStripMenuItem refreshItem)
        {
            // Navigation Group
            var groupBoxNavigation = new GroupBox
            {
                Text = "Navigation",
                Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(650, 80)
            };

            txtCurrentPath = new TextBox
            {
                Location = new Point(100, 22),
                Size = new Size(400, 20),
                ReadOnly = true,
                Text = currentPath
            };

            var btnBack = new Button
            {
                Text = "← Back",
                Location = new Point(15, 50),
                Size = new Size(70, 25)
            };

            var btnRefresh = new Button
            {
                Text = "🔄 Refresh",
                Location = new Point(95, 50),
                Size = new Size(80, 25)
            };

            var btnCreateFolder = new Button
            {
                Text = "Create Folder",
                Location = new Point(185, 50),
                Size = new Size(90, 25)
            };

            groupBoxNavigation.Controls.AddRange(new Control[] {
                new Label { Text = "Current Path:", Location = new Point(15, 25), Size = new Size(80, 20) },
                txtCurrentPath, btnBack, btnRefresh, btnCreateFolder
            });

            // File Operations Group
            var groupBoxFileOperations = new GroupBox
            {
                Text = "File Operations",
                Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold),
                Location = new Point(670, 10),
                Size = new Size(270, 80)
            };

            var btnUpload = new Button
            {
                Text = "⬆️ Upload",
                Location = new Point(10, 20),
                Size = new Size(70, 25),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            var btnDownload = new Button
            {
                Text = "⬇️ Download",
                Location = new Point(90, 20),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            var btnDelete = new Button
            {
                Text = "🗑️ Delete",
                Location = new Point(180, 20),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            progressBar = new ProgressBar
            {
                Location = new Point(10, 55),
                Size = new Size(200, 15),
                Visible = false
            };

            var lblProgress = new Label
            {
                Text = "0%",
                Location = new Point(220, 57),
                Size = new Size(30, 13),
                Font = new Font("Microsoft Sans Serif", 7F),
                Visible = false
            };

            groupBoxFileOperations.Controls.AddRange(new Control[] {
                btnUpload, btnDownload, btnDelete, progressBar, lblProgress
            });

            // File List
            lstFiles = new ListView
            {
                Location = new Point(10, 100),
                Size = new Size(930, 380),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false
            };

            lstFiles.Columns.Add("Name", 250);
            lstFiles.Columns.Add("Type", 80);
            lstFiles.Columns.Add("Size", 100);
            lstFiles.Columns.Add("Last Modified", 150);
            lstFiles.Columns.Add("Path", 300);

            // Context Menu for Files
            var contextMenuFiles = new ContextMenuStrip();
            var downloadContextItem = new ToolStripMenuItem("Download");
            var deleteContextItem = new ToolStripMenuItem("Delete");
            var propertiesContextItem = new ToolStripMenuItem("Properties");

            downloadContextItem.Click += (s, e) => DownloadFile();
            deleteContextItem.Click += (s, e) => DeleteFile();
            propertiesContextItem.Click += PropertiesToolStripMenuItem_Click;

            contextMenuFiles.Items.AddRange(new ToolStripItem[] {
                downloadContextItem, deleteContextItem, new ToolStripSeparator(), propertiesContextItem
            });
            lstFiles.ContextMenuStrip = contextMenuFiles;

            // Event handlers
            btnBack.Click += BtnBack_Click;
            btnRefresh.Click += (s, e) => LoadFileList();
            btnCreateFolder.Click += BtnCreateFolder_Click;
            btnUpload.Click += (s, e) => UploadFile();
            btnDownload.Click += (s, e) => DownloadFile();
            btnDelete.Click += (s, e) => DeleteFile();
            lstFiles.DoubleClick += LstFiles_DoubleClick;

            uploadItem.Click += (s, e) => UploadFile();
            downloadItem.Click += (s, e) => DownloadFile();
            deleteItem.Click += (s, e) => DeleteFile();
            refreshItem.Click += (s, e) => LoadFileList();

            tabPage.Controls.AddRange(new Control[] {
                groupBoxNavigation, groupBoxFileOperations, lstFiles
            });
        }

        private void CreateChatTab(TabPage tabPage)
        {
            // Chat History Group
            var groupBoxChatHistory = new GroupBox
            {
                Text = "Chat History",
                Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(930, 350)
            };

            txtChatHistory = new TextBox
            {
                Location = new Point(10, 25),
                Size = new Size(910, 315),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Consolas", 8.25F),
                BackColor = Color.White
            };

            groupBoxChatHistory.Controls.Add(txtChatHistory);

            // Message Group
            var groupBoxMessage = new GroupBox
            {
                Text = "Send Message",
                Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold),
                Location = new Point(10, 370),
                Size = new Size(930, 70)
            };

            txtMessage = new TextBox
            {
                Location = new Point(15, 25),
                Size = new Size(800, 25)
            };

            var btnSend = new Button
            {
                Text = "Send",
                Location = new Point(825, 22),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            var lblOnlineUsers = new Label
            {
                Text = "👥 Online Users: 0",
                Location = new Point(750, 5),
                Size = new Size(100, 13),
                Font = new Font("Microsoft Sans Serif", 7F),
                ForeColor = Color.Green
            };

            txtMessage.KeyPress += TxtMessage_KeyPress;
            btnSend.Click += BtnSend_Click;

            groupBoxMessage.Controls.AddRange(new Control[] { txtMessage, btnSend, lblOnlineUsers });

            tabPage.Controls.AddRange(new Control[] { groupBoxChatHistory, groupBoxMessage });
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
            toolStripStatusLabel.Text = isConnected ? "Connected" : "Disconnected";
        }

        private void ConnectToServer(string ip, int port, string username, Button btnConnect, Button btnDisconnect, ToolStripMenuItem connectItem, ToolStripMenuItem disconnectItem)
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(ip, port);
                serverStream = tcpClient.GetStream();

                SendMessage("LOGIN", username);

                clientReceiveThread = new Thread(ReceiveMessages);
                clientReceiveThread.Start();

                isConnected = true;
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                connectItem.Enabled = false;
                disconnectItem.Enabled = true;
                lblStatus.Text = "Connected";
                lblStatus.ForeColor = Color.Green;

                LoadFileList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Disconnect(Button btnConnect, Button btnDisconnect, ToolStripMenuItem connectItem, ToolStripMenuItem disconnectItem)
        {
            isConnected = false;

            try
            {
                serverStream?.Close(); // Force Read to fail
                tcpClient?.Close();
            }
            catch { }

            // Đợi thread thoát an toàn
            if (clientReceiveThread != null && clientReceiveThread.IsAlive)
            {
                clientReceiveThread.Join(1000); // Chờ tối đa 1 giây
            }

            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            connectItem.Enabled = true;
            disconnectItem.Enabled = false;
            lblStatus.Text = "Disconnected";
            lblStatus.ForeColor = Color.Red;

            lstFiles.Items.Clear();
            txtChatHistory.Clear();
        }



        private void ReceiveMessages()
        {
            var message = new byte[8192];
            while (isConnected)
            {
                try
                {
                    int bytesRead = serverStream.Read(message, 0, message.Length);
                    if (bytesRead == 0) break;

                    string jsonMessage = Encoding.UTF8.GetString(message, 0, bytesRead);
                    ProcessServerMessage(jsonMessage);
                }
                catch
                {
                    if (isConnected)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show("Connection lost!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            var btnConnect = this.Controls.OfType<GroupBox>().First().Controls.OfType<Button>().First(b => b.Text == "Connect");
                            var btnDisconnect = this.Controls.OfType<GroupBox>().First().Controls.OfType<Button>().First(b => b.Text == "Disconnect");
                            var menuStrip = this.Controls.OfType<MenuStrip>().First();
                            var connectItem = menuStrip.Items.OfType<ToolStripMenuItem>().First().DropDownItems.OfType<ToolStripMenuItem>().First(i => i.Text == "Connect");
                            var disconnectItem = menuStrip.Items.OfType<ToolStripMenuItem>().First().DropDownItems.OfType<ToolStripMenuItem>().First(i => i.Text == "Disconnect");
                            Disconnect(btnConnect, btnDisconnect, connectItem, disconnectItem);
                        });
                    }
                    break;
                }
            }
        }


        private void ProcessServerMessage(string jsonMessage)
        {
            try
            {
                var response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonMessage);
                string type = response["Type"].ToString();
                string status = response["Status"].ToString();
                string data = response.ContainsKey("Data") ? response["Data"].ToString() : "";

                this.Invoke((MethodInvoker)delegate
                {
                    switch (type)
                    {
                        case "LIST_FILES":
                            if (status == "OK")
                            {
                                var files = JsonSerializer.Deserialize<object[]>(data);
                                UpdateFileList(files);
                            }
                            else
                            {
                                MessageBox.Show($"Error loading files: {data}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;

                        case "DOWNLOAD_FILE":
                            if (status == "OK")
                            {
                                string filePath = response.ContainsKey("Data") ? response["Data"].ToString() : "unknown";
                                string base64Data = response.ContainsKey("FileData") ? response["FileData"].ToString() : null;

                                if (!string.IsNullOrEmpty(base64Data))
                                {
                                    SaveDownloadedFile(filePath, base64Data);
                                }
                                else
                                {
                                    MessageBox.Show("Download failed: File data is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                string errorMessage = response.ContainsKey("Data") ? response["Data"].ToString() : "No error message from server.";

                                string debugInfo = $"Status: {status}\n" +
                                                   $"Keys: {string.Join(", ", response.Keys)}\n" +
                                                   $"FileData Preview: N/A\n" +
                                                   $"Server Message: {errorMessage}";

                                MessageBox.Show($"Download failed.\n\n{debugInfo}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;

                        case "UPLOAD_FILE":
                            if (status == "OK")
                            {
                                MessageBox.Show(data, "Upload Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadFileList(); // Cập nhật danh sách file sau khi upload
                            }
                            else
                            {
                                string errorMessage = !string.IsNullOrEmpty(data) ? data : "Unknown upload error.";
                                MessageBox.Show($"Upload failed: {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;

                        case "DELETE_FILE":
                            if (status == "OK")
                            {
                                MessageBox.Show(data, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadFileList();
                            }
                            else
                            {
                                MessageBox.Show($"Operation failed: {data}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;

                        case "CHAT_MESSAGE":
                            txtChatHistory.AppendText($"[{DateTime.Now:HH:mm:ss}] {data}\r\n");
                            txtChatHistory.ScrollToCaret();
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show($"Error processing message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        }

        private void SendMessage(string type, string data, string fileData = null)
        {
            try
            {
                var message = new Dictionary<string, object> { ["Type"] = type, ["Data"] = data };
                if (fileData != null) message["FileData"] = fileData;

                string jsonMessage = JsonSerializer.Serialize(message);
                byte[] messageBytes = Encoding.UTF8.GetBytes(jsonMessage);
                serverStream.Write(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFileList()
        {
            if (isConnected) SendMessage("LIST_FILES", currentPath);
        }

        private void UpdateFileList(object[] files)
        {
            lstFiles.Items.Clear();
            foreach (var fileObj in files)
            {
                var file = JsonSerializer.Deserialize<Dictionary<string, object>>(fileObj.ToString());
                var item = new ListViewItem(file["Name"].ToString());
                item.SubItems.Add(file["Type"].ToString());

                if (file["Type"].ToString() == "file")
                {
                    long size = ((JsonElement)file["Size"]).GetInt64();
                    item.SubItems.Add(FormatFileSize(size));
                }
                else
                {
                    item.SubItems.Add("");
                }

                item.SubItems.Add(file["LastModified"].ToString());
                item.SubItems.Add(file["Path"].ToString());
                item.Tag = file;

                if (file["Type"].ToString() == "folder")
                    item.ForeColor = Color.Blue;

                lstFiles.Items.Add(item);
            }
            txtCurrentPath.Text = currentPath;
        }

        private void LstFiles_DoubleClick(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItems.Count > 0)
            {
                var file = (Dictionary<string, object>)lstFiles.SelectedItems[0].Tag;
                if (file["Type"].ToString() == "folder")
                {
                    currentPath = file["Path"].ToString();
                    LoadFileList();
                }
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (currentPath != "/")
            {
                string[] parts = currentPath.Split('/');
                currentPath = string.Join("/", parts, 0, parts.Length - 1);
                if (string.IsNullOrEmpty(currentPath)) currentPath = "/";
                LoadFileList();
            }
        }

        private void BtnCreateFolder_Click(object sender, EventArgs e)
        {
            var folderName = Microsoft.VisualBasic.Interaction.InputBox("Enter folder name:", "Create Folder", "New Folder");
            if (!string.IsNullOrEmpty(folderName))
            {
                // Implementation would require server support for folder creation
                MessageBox.Show("Folder creation not implemented yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UploadFile()
        {
            if (!isConnected) return;

            using (var dialog = new OpenFileDialog { Filter = "All Files (*.*)|*.*", Title = "Select File to Upload" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        progressBar.Visible = true;
                        progressBar.Value = 50;

                        byte[] fileData = File.ReadAllBytes(dialog.FileName);
                        string base64Data = Convert.ToBase64String(fileData);
                        string fileName = Path.GetFileName(dialog.FileName);
                        string serverPath = currentPath == "/" ? $"/{fileName}" : $"{currentPath}/{fileName}";

                        SendMessage("UPLOAD_FILE", serverPath, base64Data);
                        progressBar.Value = 100;

                        Thread.Sleep(1000);
                        progressBar.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        progressBar.Visible = false;
                        MessageBox.Show($"Upload failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DownloadFile()
        {
            if (!isConnected || lstFiles.SelectedItems.Count == 0) return;

            var file = (Dictionary<string, object>)lstFiles.SelectedItems[0].Tag;
            if (file["Type"].ToString() == "file")
            {
                progressBar.Visible = true;
                progressBar.Value = 25;
                string path = file["Path"].ToString();
                MessageBox.Show($"Đang yêu cầu tải file: {path}", "Debug");
                SendMessage("DOWNLOAD_FILE", path);

            }
        }

        private void SaveDownloadedFile(string filePath, string base64Data)
        {
            try
            {
                byte[] fileBytes = Convert.FromBase64String(base64Data);
                string fileName = Path.GetFileName(filePath);

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = fileName,
                    Filter = "All Files|*.*"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, fileBytes);
                    MessageBox.Show("File downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DeleteFile()
        {
            if (!isConnected || lstFiles.SelectedItems.Count == 0) return;

            var file = (Dictionary<string, object>)lstFiles.SelectedItems[0].Tag;
            var result = MessageBox.Show($"Are you sure you want to delete '{file["Name"]}'?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                SendMessage("DELETE_FILE", file["Path"].ToString());
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            SendChatMessage();
        }

        private void TxtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendChatMessage();
                e.Handled = true;
            }
        }

        private void SendChatMessage()
        {
            if (!isConnected || string.IsNullOrWhiteSpace(txtMessage.Text)) return;

            string message = txtMessage.Text.Trim();
            SendMessage("CHAT_MESSAGE", message);

            txtChatHistory.AppendText($"[{DateTime.Now:HH:mm:ss}] You: {message}\r\n");
            txtChatHistory.ScrollToCaret();
            txtMessage.Clear();
        }

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItems.Count > 0)
            {
                var file = (Dictionary<string, object>)lstFiles.SelectedItems[0].Tag;
                var properties = $"File Properties:\n\n" +
                               $"Name: {file["Name"]}\n" +
                               $"Type: {file["Type"]}\n" +
                               $"Path: {file["Path"]}\n" +
                               $"Last Modified: {file["LastModified"]}";

                if (file["Type"].ToString() == "file")
                {
                    properties += $"\nSize: {FormatFileSize(Convert.ToInt64(file["Size"]))}";
                }

                MessageBox.Show(properties, "Properties", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("File Manager Client v1.0\n\nA simple file sharing client with chat functionality.\n\nUsing System.Text.Json for improved performance.",
                           "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string FormatFileSize(long bytes)
        {
            if (bytes == 0) return "0 Bytes";
            string[] sizes = { "Bytes", "KB", "MB", "GB" };
            int i = (int)Math.Floor(Math.Log(bytes) / Math.Log(1024));
            return Math.Round(bytes / Math.Pow(1024, i), 2) + " " + sizes[i];
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isConnected)
            {
                // Find controls and disconnect
                var btnConnect = this.Controls.OfType<GroupBox>().First().Controls.OfType<Button>().First(b => b.Text == "Connect");
                var btnDisconnect = this.Controls.OfType<GroupBox>().First().Controls.OfType<Button>().First(b => b.Text == "Disconnect");
                var menuStrip = this.Controls.OfType<MenuStrip>().First();
                var connectItem = menuStrip.Items.OfType<ToolStripMenuItem>().First().DropDownItems.OfType<ToolStripMenuItem>().First(i => i.Text == "Connect");
                var disconnectItem = menuStrip.Items.OfType<ToolStripMenuItem>().First().DropDownItems.OfType<ToolStripMenuItem>().First(i => i.Text == "Disconnect");
                Disconnect(btnConnect, btnDisconnect, connectItem, disconnectItem);
            }
            base.OnFormClosing(e);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {

        }
    }
}
