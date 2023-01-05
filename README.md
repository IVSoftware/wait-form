One of many ways to show a **Winform waiting panel** is to run the `LoadData` method on a Task and give the `WaitFormWaitingPanel` the ability to manage its own visibility.

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            buttonLoadData.Click += onClickLoadData;
        }

        private async void onClickLoadData(object? sender, EventArgs e)
        {
            using (var waitForm = new WaitFormWaitingPanel(this))
            {
                await Task.Run(() => LoadData());
            }
        }
        private void LoadData()
        {
            // Mock "any" long-running synchronous task
            // (SQL or otherwise)
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }

***
**Example**
[![screenshot][1]][1]

    class WaitFormWaitingPanel : Form
    {
        public WaitFormWaitingPanel(Form owner)
        {
            Owner = owner;
            var label = new Label
            {
                Text = "Loading...",
                TextAlign = ContentAlignment.MiddleCenter,
                Size = Owner.ClientRectangle.Size,
            };
            Controls.Add(label);
            Size = Owner.ClientRectangle.Size;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.LightBlue;
            UseWaitCursor = true;
            StartPosition = FormStartPosition.Manual;
            Location = PointToClient(Owner.PointToScreen(Owner.ClientRectangle.Location));
            var forceHandle = Handle;
            BeginInvoke(() => ShowDialog());
            Opacity = .5;
        }
    }

  [1]: https://i.stack.imgur.com/dbn0z.png