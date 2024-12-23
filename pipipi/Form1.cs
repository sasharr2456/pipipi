namespace pipipi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button_enter_Click(object sender, EventArgs e)
        {
            AuthForm form = new AuthForm("add");
            form.ShowDialog();
        }

        private void button_reg_Click(object sender, EventArgs e)
        {
            AuthForm form = new AuthForm("reg");
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AuthForm form = new AuthForm("add");
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AuthForm form = new AuthForm("reg");
            form.ShowDialog();
        }
    }
}