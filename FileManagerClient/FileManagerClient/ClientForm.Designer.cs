namespace FileManagerClient
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            connectToolStripMenuItem = new ToolStripMenuItem();
            disconnectToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            uploadFileToolStripMenuItem = new ToolStripMenuItem();
            downloadFileToolStripMenuItem = new ToolStripMenuItem();
            deleteFileToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripStatusConnection = new ToolStripStatusLabel();
            groupBoxConnection = new GroupBox();
            lblStatus = new Label();
            btnDisconnect = new Button();
            btnConnect = new Button();
            txtUsername = new TextBox();
            label3 = new Label();
            txtPort = new TextBox();
            label2 = new Label();
            txtServerIP = new TextBox();
            label1 = new Label();
            tabControl1 = new TabControl();
            tabPageFiles = new TabPage();
            lstFiles = new ListView();
            columnName = new ColumnHeader();
            columnType = new ColumnHeader();
            columnSize = new ColumnHeader();
            columnModified = new ColumnHeader();
            columnPath = new ColumnHeader();
            contextMenuFiles = new ContextMenuStrip(components);
            downloadToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            propertiesToolStripMenuItem = new ToolStripMenuItem();
            groupBoxFileOperations = new GroupBox();
            progressBar1 = new ProgressBar();
            btnDelete = new Button();
            btnDownload = new Button();
            btnUpload = new Button();
            groupBoxNavigation = new GroupBox();
            btnCreateFolder = new Button();
            btnRefresh = new Button();
            btnBack = new Button();
            txtCurrentPath = new TextBox();
            label4 = new Label();
            tabPageChat = new TabPage();
            groupBoxMessage = new GroupBox();
            btnSend = new Button();
            txtMessage = new TextBox();
            groupBoxChatHistory = new GroupBox();
            txtChatHistory = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            groupBoxConnection.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageFiles.SuspendLayout();
            contextMenuFiles.SuspendLayout();
            groupBoxFileOperations.SuspendLayout();
            groupBoxNavigation.SuspendLayout();
            tabPageChat.SuspendLayout();
            groupBoxMessage.SuspendLayout();
            groupBoxChatHistory.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, viewToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(8, 3, 0, 3);
            menuStrip1.Size = new Size(1333, 30);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { connectToolStripMenuItem, disconnectToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // connectToolStripMenuItem
            // 
            connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            connectToolStripMenuItem.ShortcutKeys = Keys.F5;
            connectToolStripMenuItem.Size = new Size(189, 26);
            connectToolStripMenuItem.Text = "Connect";
            // 
            // disconnectToolStripMenuItem
            // 
            disconnectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            disconnectToolStripMenuItem.ShortcutKeys = Keys.F6;
            disconnectToolStripMenuItem.Size = new Size(189, 26);
            disconnectToolStripMenuItem.Text = "Disconnect";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(186, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitToolStripMenuItem.Size = new Size(189, 26);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { uploadFileToolStripMenuItem, downloadFileToolStripMenuItem, deleteFileToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(49, 24);
            editToolStripMenuItem.Text = "Edit";
            // 
            // uploadFileToolStripMenuItem
            // 
            uploadFileToolStripMenuItem.Name = "uploadFileToolStripMenuItem";
            uploadFileToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.U;
            uploadFileToolStripMenuItem.Size = new Size(241, 26);
            uploadFileToolStripMenuItem.Text = "Upload File";
            // 
            // downloadFileToolStripMenuItem
            // 
            downloadFileToolStripMenuItem.Name = "downloadFileToolStripMenuItem";
            downloadFileToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
            downloadFileToolStripMenuItem.Size = new Size(241, 26);
            downloadFileToolStripMenuItem.Text = "Download File";
            // 
            // deleteFileToolStripMenuItem
            // 
            deleteFileToolStripMenuItem.Name = "deleteFileToolStripMenuItem";
            deleteFileToolStripMenuItem.ShortcutKeys = Keys.Delete;
            deleteFileToolStripMenuItem.Size = new Size(241, 26);
            deleteFileToolStripMenuItem.Text = "Delete File";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { refreshToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(55, 24);
            viewToolStripMenuItem.Text = "View";
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.ShortcutKeys = Keys.F5;
            refreshToolStripMenuItem.Size = new Size(165, 26);
            refreshToolStripMenuItem.Text = "Refresh";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(133, 26);
            aboutToolStripMenuItem.Text = "About";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripStatusConnection });
            statusStrip1.Location = new Point(0, 1029);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 19, 0);
            statusStrip1.Size = new Size(1333, 26);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(50, 20);
            toolStripStatusLabel1.Text = "Ready";
            // 
            // toolStripStatusConnection
            // 
            toolStripStatusConnection.Name = "toolStripStatusConnection";
            toolStripStatusConnection.Size = new Size(99, 20);
            toolStripStatusConnection.Text = "Disconnected";
            // 
            // groupBoxConnection
            // 
            groupBoxConnection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxConnection.Controls.Add(lblStatus);
            groupBoxConnection.Controls.Add(btnDisconnect);
            groupBoxConnection.Controls.Add(btnConnect);
            groupBoxConnection.Controls.Add(txtUsername);
            groupBoxConnection.Controls.Add(label3);
            groupBoxConnection.Controls.Add(txtPort);
            groupBoxConnection.Controls.Add(label2);
            groupBoxConnection.Controls.Add(txtServerIP);
            groupBoxConnection.Controls.Add(label1);
            groupBoxConnection.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxConnection.Location = new Point(16, 46);
            groupBoxConnection.Margin = new Padding(4, 5, 4, 5);
            groupBoxConnection.Name = "groupBoxConnection";
            groupBoxConnection.Padding = new Padding(4, 5, 4, 5);
            groupBoxConnection.Size = new Size(1301, 123);
            groupBoxConnection.TabIndex = 2;
            groupBoxConnection.TabStop = false;
            groupBoxConnection.Text = "Server Connection";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(933, 51);
            lblStatus.Margin = new Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(124, 20);
            lblStatus.TabIndex = 8;
            lblStatus.Text = "Disconnected";
            // 
            // btnDisconnect
            // 
            btnDisconnect.BackColor = Color.FromArgb(220, 53, 69);
            btnDisconnect.Enabled = false;
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.ForeColor = Color.White;
            btnDisconnect.Location = new Point(787, 38);
            btnDisconnect.Margin = new Padding(4, 5, 4, 5);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(120, 46);
            btnDisconnect.TabIndex = 7;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = false;
            // 
            // btnConnect
            // 
            btnConnect.BackColor = Color.FromArgb(40, 167, 69);
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(667, 38);
            btnConnect.Margin = new Padding(4, 5, 4, 5);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(107, 46);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = false;
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(520, 46);
            txtUsername.Margin = new Padding(4, 5, 4, 5);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(132, 23);
            txtUsername.TabIndex = 5;
            txtUsername.Text = "User1";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(427, 51);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(77, 17);
            label3.TabIndex = 4;
            label3.Text = "Username:";
            // 
            // txtPort
            // 
            txtPort.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPort.Location = new Point(333, 46);
            txtPort.Margin = new Padding(4, 5, 4, 5);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(79, 23);
            txtPort.TabIndex = 3;
            txtPort.Text = "8080";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(280, 51);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(38, 17);
            label2.TabIndex = 2;
            label2.Text = "Port:";
            // 
            // txtServerIP
            // 
            txtServerIP.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtServerIP.Location = new Point(107, 46);
            txtServerIP.Margin = new Padding(4, 5, 4, 5);
            txtServerIP.Name = "txtServerIP";
            txtServerIP.Size = new Size(159, 23);
            txtServerIP.TabIndex = 1;
            txtServerIP.Text = "127.0.0.1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(20, 51);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(70, 17);
            label1.TabIndex = 0;
            label1.Text = "Server IP:";
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPageFiles);
            tabControl1.Controls.Add(tabPageChat);
            tabControl1.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(16, 185);
            tabControl1.Margin = new Padding(4, 5, 4, 5);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1301, 846);
            tabControl1.TabIndex = 3;
            // 
            // tabPageFiles
            // 
            tabPageFiles.Controls.Add(lstFiles);
            tabPageFiles.Controls.Add(groupBoxFileOperations);
            tabPageFiles.Controls.Add(groupBoxNavigation);
            tabPageFiles.Location = new Point(4, 27);
            tabPageFiles.Margin = new Padding(4, 5, 4, 5);
            tabPageFiles.Name = "tabPageFiles";
            tabPageFiles.Padding = new Padding(4, 5, 4, 5);
            tabPageFiles.Size = new Size(1293, 815);
            tabPageFiles.TabIndex = 0;
            tabPageFiles.Text = "📁 File Manager";
            tabPageFiles.UseVisualStyleBackColor = true;
            // 
            // lstFiles
            // 
            lstFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstFiles.Columns.AddRange(new ColumnHeader[] { columnName, columnType, columnSize, columnModified, columnPath });
            lstFiles.ContextMenuStrip = contextMenuFiles;
            lstFiles.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lstFiles.FullRowSelect = true;
            lstFiles.GridLines = true;
            lstFiles.Location = new Point(13, 154);
            lstFiles.Margin = new Padding(4, 5, 4, 5);
            lstFiles.MultiSelect = false;
            lstFiles.Name = "lstFiles";
            lstFiles.Size = new Size(1265, 636);
            lstFiles.TabIndex = 2;
            lstFiles.UseCompatibleStateImageBehavior = false;
            lstFiles.View = View.Details;
            // 
            // columnName
            // 
            columnName.Text = "Name";
            columnName.Width = 250;
            // 
            // columnType
            // 
            columnType.Text = "Type";
            columnType.Width = 80;
            // 
            // columnSize
            // 
            columnSize.Text = "Size";
            columnSize.Width = 100;
            // 
            // columnModified
            // 
            columnModified.Text = "Last Modified";
            columnModified.Width = 150;
            // 
            // columnPath
            // 
            columnPath.Text = "Path";
            columnPath.Width = 300;
            // 
            // contextMenuFiles
            // 
            contextMenuFiles.ImageScalingSize = new Size(20, 20);
            contextMenuFiles.Items.AddRange(new ToolStripItem[] { downloadToolStripMenuItem, deleteToolStripMenuItem, toolStripSeparator2, propertiesToolStripMenuItem });
            contextMenuFiles.Name = "contextMenuFiles";
            contextMenuFiles.Size = new Size(148, 82);
            // 
            // downloadToolStripMenuItem
            // 
            downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            downloadToolStripMenuItem.Size = new Size(147, 24);
            downloadToolStripMenuItem.Text = "Download";
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(147, 24);
            deleteToolStripMenuItem.Text = "Delete";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(144, 6);
            // 
            // propertiesToolStripMenuItem
            // 
            propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            propertiesToolStripMenuItem.Size = new Size(147, 24);
            propertiesToolStripMenuItem.Text = "Properties";
            // 
            // groupBoxFileOperations
            // 
            groupBoxFileOperations.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxFileOperations.Controls.Add(progressBar1);
            groupBoxFileOperations.Controls.Add(btnDelete);
            groupBoxFileOperations.Controls.Add(btnDownload);
            groupBoxFileOperations.Controls.Add(btnUpload);
            groupBoxFileOperations.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxFileOperations.Location = new Point(893, 15);
            groupBoxFileOperations.Margin = new Padding(4, 5, 4, 5);
            groupBoxFileOperations.Name = "groupBoxFileOperations";
            groupBoxFileOperations.Padding = new Padding(4, 5, 4, 5);
            groupBoxFileOperations.Size = new Size(387, 123);
            groupBoxFileOperations.TabIndex = 1;
            groupBoxFileOperations.TabStop = false;
            groupBoxFileOperations.Text = "File Operations";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(13, 85);
            progressBar1.Margin = new Padding(4, 5, 4, 5);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(360, 23);
            progressBar1.TabIndex = 3;
            progressBar1.Visible = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(253, 31);
            btnDelete.Margin = new Padding(4, 5, 4, 5);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(107, 38);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "🗑️ Delete";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnDownload
            // 
            btnDownload.BackColor = Color.FromArgb(23, 162, 184);
            btnDownload.FlatStyle = FlatStyle.Flat;
            btnDownload.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDownload.ForeColor = Color.White;
            btnDownload.Location = new Point(133, 31);
            btnDownload.Margin = new Padding(4, 5, 4, 5);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(107, 38);
            btnDownload.TabIndex = 1;
            btnDownload.Text = "⬇️ Download";
            btnDownload.UseVisualStyleBackColor = false;
            btnDownload.Click += btnDownload_Click;
            // 
            // btnUpload
            // 
            btnUpload.BackColor = Color.FromArgb(40, 167, 69);
            btnUpload.FlatStyle = FlatStyle.Flat;
            btnUpload.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpload.ForeColor = Color.White;
            btnUpload.Location = new Point(13, 31);
            btnUpload.Margin = new Padding(4, 5, 4, 5);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(107, 38);
            btnUpload.TabIndex = 0;
            btnUpload.Text = "⬆️ Upload";
            btnUpload.UseVisualStyleBackColor = false;
            // 
            // groupBoxNavigation
            // 
            groupBoxNavigation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxNavigation.Controls.Add(btnCreateFolder);
            groupBoxNavigation.Controls.Add(btnRefresh);
            groupBoxNavigation.Controls.Add(btnBack);
            groupBoxNavigation.Controls.Add(txtCurrentPath);
            groupBoxNavigation.Controls.Add(label4);
            groupBoxNavigation.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxNavigation.Location = new Point(13, 15);
            groupBoxNavigation.Margin = new Padding(4, 5, 4, 5);
            groupBoxNavigation.Name = "groupBoxNavigation";
            groupBoxNavigation.Padding = new Padding(4, 5, 4, 5);
            groupBoxNavigation.Size = new Size(867, 123);
            groupBoxNavigation.TabIndex = 0;
            groupBoxNavigation.TabStop = false;
            groupBoxNavigation.Text = "Navigation";
            // 
            // btnCreateFolder
            // 
            btnCreateFolder.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCreateFolder.Location = new Point(247, 77);
            btnCreateFolder.Margin = new Padding(4, 5, 4, 5);
            btnCreateFolder.Name = "btnCreateFolder";
            btnCreateFolder.Size = new Size(120, 38);
            btnCreateFolder.TabIndex = 4;
            btnCreateFolder.Text = "Create Folder";
            btnCreateFolder.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRefresh.Location = new Point(127, 77);
            btnRefresh.Margin = new Padding(4, 5, 4, 5);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(107, 38);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            btnBack.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBack.Location = new Point(20, 77);
            btnBack.Margin = new Padding(4, 5, 4, 5);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(93, 38);
            btnBack.TabIndex = 2;
            btnBack.Text = "← Back";
            btnBack.UseVisualStyleBackColor = true;
            // 
            // txtCurrentPath
            // 
            txtCurrentPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCurrentPath.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCurrentPath.Location = new Point(133, 34);
            txtCurrentPath.Margin = new Padding(4, 5, 4, 5);
            txtCurrentPath.Name = "txtCurrentPath";
            txtCurrentPath.ReadOnly = true;
            txtCurrentPath.Size = new Size(719, 23);
            txtCurrentPath.TabIndex = 1;
            txtCurrentPath.Text = "/";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(20, 38);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(92, 17);
            label4.TabIndex = 0;
            label4.Text = "Current Path:";
            // 
            // tabPageChat
            // 
            tabPageChat.Controls.Add(groupBoxMessage);
            tabPageChat.Controls.Add(groupBoxChatHistory);
            tabPageChat.Location = new Point(4, 27);
            tabPageChat.Margin = new Padding(4, 5, 4, 5);
            tabPageChat.Name = "tabPageChat";
            tabPageChat.Padding = new Padding(4, 5, 4, 5);
            tabPageChat.Size = new Size(1293, 815);
            tabPageChat.TabIndex = 1;
            tabPageChat.Text = "💬 Chat";
            tabPageChat.UseVisualStyleBackColor = true;
            // 
            // groupBoxMessage
            // 
            groupBoxMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxMessage.Controls.Add(btnSend);
            groupBoxMessage.Controls.Add(txtMessage);
            groupBoxMessage.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxMessage.Location = new Point(13, 677);
            groupBoxMessage.Margin = new Padding(4, 5, 4, 5);
            groupBoxMessage.Name = "groupBoxMessage";
            groupBoxMessage.Padding = new Padding(4, 5, 4, 5);
            groupBoxMessage.Size = new Size(1267, 108);
            groupBoxMessage.TabIndex = 1;
            groupBoxMessage.TabStop = false;
            groupBoxMessage.Text = "Send Message";
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSend.BackColor = Color.FromArgb(0, 123, 255);
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(1133, 34);
            btnSend.Margin = new Padding(4, 5, 4, 5);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(120, 46);
            btnSend.TabIndex = 1;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = false;
            // 
            // txtMessage
            // 
            txtMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtMessage.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMessage.Location = new Point(20, 38);
            txtMessage.Margin = new Padding(4, 5, 4, 5);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(1099, 23);
            txtMessage.TabIndex = 0;
            // 
            // groupBoxChatHistory
            // 
            groupBoxChatHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxChatHistory.Controls.Add(txtChatHistory);
            groupBoxChatHistory.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxChatHistory.Location = new Point(13, 15);
            groupBoxChatHistory.Margin = new Padding(4, 5, 4, 5);
            groupBoxChatHistory.Name = "groupBoxChatHistory";
            groupBoxChatHistory.Padding = new Padding(4, 5, 4, 5);
            groupBoxChatHistory.Size = new Size(1267, 646);
            groupBoxChatHistory.TabIndex = 0;
            groupBoxChatHistory.TabStop = false;
            groupBoxChatHistory.Text = "Chat History";
            // 
            // txtChatHistory
            // 
            txtChatHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtChatHistory.BackColor = Color.White;
            txtChatHistory.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtChatHistory.Location = new Point(13, 38);
            txtChatHistory.Margin = new Padding(4, 5, 4, 5);
            txtChatHistory.Multiline = true;
            txtChatHistory.Name = "txtChatHistory";
            txtChatHistory.ReadOnly = true;
            txtChatHistory.ScrollBars = ScrollBars.Vertical;
            txtChatHistory.Size = new Size(1239, 590);
            txtChatHistory.TabIndex = 0;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1333, 1055);
            Controls.Add(tabControl1);
            Controls.Add(groupBoxConnection);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(1194, 898);
            Name = "ClientForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "File Manager Client v1.0";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            groupBoxConnection.ResumeLayout(false);
            groupBoxConnection.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPageFiles.ResumeLayout(false);
            contextMenuFiles.ResumeLayout(false);
            groupBoxFileOperations.ResumeLayout(false);
            groupBoxNavigation.ResumeLayout(false);
            groupBoxNavigation.PerformLayout();
            tabPageChat.ResumeLayout(false);
            groupBoxMessage.ResumeLayout(false);
            groupBoxMessage.PerformLayout();
            groupBoxChatHistory.ResumeLayout(false);
            groupBoxChatHistory.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusConnection;
        private System.Windows.Forms.GroupBox groupBoxConnection;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageFiles;
        private System.Windows.Forms.ListView lstFiles;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.ColumnHeader columnModified;
        private System.Windows.Forms.ColumnHeader columnPath;
        private System.Windows.Forms.ContextMenuStrip contextMenuFiles;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxFileOperations;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.GroupBox groupBoxNavigation;
        private System.Windows.Forms.Button btnCreateFolder;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.TextBox txtCurrentPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPageChat;
        private System.Windows.Forms.GroupBox groupBoxMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.GroupBox groupBoxChatHistory;
        private System.Windows.Forms.TextBox txtChatHistory;
        private System.Windows.Forms.Timer timer1;
    }
}
