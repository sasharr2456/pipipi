using System;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using pipipi;

public class AuthForm : Form
{
    private Label lblUsername;
    private Label lblPassword;
    private Label lblFullName;
    private Label lblDateOfBirth; // New label for Date of Birth
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.TextBox txtFullName;
    private System.Windows.Forms.Button btnSubmit;
    private DateTimePicker dateTimePicker1; // New DateTimePicker for Date of Birth
    private string action;

    public AuthForm(string action)
    {
        this.action = action;

        this.Size = new System.Drawing.Size(400, action == "reg" ? 450 : 350); // Adjust size for DateTimePicker
        this.Text = action == "reg" ? "Регистрация" : "Вход";

        lblUsername = new Label()
        {
            Text = "Логин:",
            Location = new System.Drawing.Point(50, 100),
            Size = new System.Drawing.Size(100, 20),
            Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
        };

        lblPassword = new Label()
        {
            Text = "Пароль:",
            Location = new System.Drawing.Point(50, 150),
            Size = new System.Drawing.Size(100, 20),
            Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
        };

        txtUsername = new System.Windows.Forms.TextBox()
        {
            Location = new System.Drawing.Point(150, 100),
            Width = 200,
            Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
        };

        txtPassword = new System.Windows.Forms.TextBox()
        {
            Location = new System.Drawing.Point(150, 150),
            Width = 200,
            PasswordChar = '*',
            Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
        };

        if (action == "reg")
        {
            lblFullName = new Label()
            {
                Text = "ФИО:",
                Location = new System.Drawing.Point(50, 200),
                Size = new System.Drawing.Size(100, 20),
                Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
            };

            txtFullName = new System.Windows.Forms.TextBox()
            {
                Location = new System.Drawing.Point(150, 200),
                Width = 200,
                Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
            };

            // Add DateTimePicker for Date of Birth
            lblDateOfBirth = new Label()
            {
                Text = "Дата рождения:",
                Location = new System.Drawing.Point(50, 250),
                Size = new System.Drawing.Size(100, 20),
                Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
            };

            dateTimePicker1 = new DateTimePicker()
            {
                Location = new System.Drawing.Point(150, 250),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
            };

            this.Controls.Add(lblFullName);
            this.Controls.Add(txtFullName);
            this.Controls.Add(lblDateOfBirth);
            this.Controls.Add(dateTimePicker1); // Add DateTimePicker to the form
        }

        btnSubmit = new System.Windows.Forms.Button()
        {
            Text = action == "reg" ? "Зарегистрироваться" : "Войти",
            Location = new System.Drawing.Point(100, action == "reg" ? 300 : 200), // Adjust button position
            Width = 200,
            Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
        };

        btnSubmit.Click += BtnSubmit_Click;

        this.Controls.Add(lblUsername);
        this.Controls.Add(lblPassword);
        this.Controls.Add(txtUsername);
        this.Controls.Add(txtPassword);
        this.Controls.Add(btnSubmit);
    }

    private void BtnSubmit_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;
        string fullName = action == "reg" ? txtFullName.Text : null;
        DateTime? dateOfBirth = action == "reg" ? dateTimePicker1.Value : (DateTime?)null; // Get the selected date

        if (action == "reg")
        {
            RegisterUser(username, password, fullName, dateOfBirth);
        }
        else if (action == "add")
        {
            LoginUser(username, password);
        }
    }

    private void RegisterUser(string username, string password, string fullName, DateTime? dateOfBirth)
    {
        string connectionString = @"Server=ALEKXSANDR;Database=БД;Trusted_Connection=True;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO  Students (FULLName,  DateOfBirth, Email, Passvord) VALUES (@FullName , @DateOfBirth, @Email, @Passvord  )";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", username);
            command.Parameters.AddWithValue("@Passvord", password);
            command.Parameters.AddWithValue("@FullName", fullName);
            command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth.HasValue ? (object)dateOfBirth.Value : DBNull.Value); // Handle nullable DateTime
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Пользователь зарегистрирован.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }
    }

    private void LoginUser(string username, string password)
    {
        string connectionString = @"Server=ALEKXSANDR;Database=БД;Trusted_Connection=True";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT UserID FROM Users WHERE Login = @Username AND Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            connection.Open();
            object result = command.ExecuteScalar();

            if (result != null)
            {
                int userId = Convert.ToInt32(result);
                MessageBox.Show("Вход выполнен успешно.");
                Hide();
                menu form = new menu(userId);
                form.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }
    }
}
