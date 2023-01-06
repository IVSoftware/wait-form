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
            using (var waitForm = new WinFormWaitingPanel(this))
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

    class WinFormWaitingPanel : Form
    {
        public WinFormWaitingPanel(Form owner)
        {
            Owner = owner;

            // Size this form to cover the main form's client rectangle.
            Size = Owner.ClientRectangle.Size;
            FormBorderStyle = FormBorderStyle.None;
            UseWaitCursor = true;
            StartPosition = FormStartPosition.Manual;
            Location = PointToClient(
                Owner.PointToScreen(Owner.ClientRectangle.Location));

            // Add a label that says "Loading..."
            var label = new Label
            {
                Text = "Loading...",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
            };
            Controls.Add(label);

            // Set the form color to see-through Blue
            BackColor = Color.LightBlue;
            Opacity = .5;

            var forceHandle = Handle;
            BeginInvoke(() => ShowDialog());
        }
    }

  [1]: https://i.stack.imgur.com/dbn0z.png