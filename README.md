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
            StartPosition = FormStartPosition.CenterParent;
            var forceHandle = Handle;
            var label = new Label
            {
                Text = "Loading...",
                Size = new Size(Owner.ClientRectangle.Width, Owner.ClientRectangle.Height),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(label);
            Size = new Size(label.Width + 20, label.Height + 20);
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.LightBlue;
            UseWaitCursor = true;
            BeginInvoke(() => ShowDialog());
            Opacity = .5;
        }
    }


  [1]: https://i.stack.imgur.com/dbn0z.png