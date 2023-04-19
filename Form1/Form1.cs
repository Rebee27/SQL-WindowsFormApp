using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing.Printing;

namespace Lab1
{
    public partial class Form1 : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

        private string childTableName = ConfigurationManager.AppSettings["childTableName"];
        private string childColumnNames = ConfigurationManager.AppSettings["childColumnNames"];
        private int nrColumnsChild = Int32.Parse(ConfigurationManager.AppSettings["childNrColumns"]);
        private string childColumnParam = ConfigurationManager.AppSettings["childColumnNamesInsertParam"];

        private string parentTableName = ConfigurationManager.AppSettings["parentTableName"];
        private string parentColumnNames = ConfigurationManager.AppSettings["parentColumnNames"];
        private int nrColumnsParent = Int32.Parse(ConfigurationManager.AppSettings["parentNrColumns"]);
        
        private string selectParentTable = ConfigurationManager.AppSettings["selectAllParent"];
        private string selectChildTable = ConfigurationManager.AppSettings["selectAllChild"];

        private List<string> parentColumnNamesList = new List<string>(ConfigurationManager.AppSettings["parentColumnNames"].Split(','));
        private List<string> childColumnNamesList = new List<string>(ConfigurationManager.AppSettings["childColumnNames"].Split(','));
        private List<string> childColumnNamesParamList = new List<string>(ConfigurationManager.AppSettings["childColumnNamesInsertParam"].Split(','));

        private SqlConnection connection = new SqlConnection();

        // Create DataSet
        private DataSet dataSet = new DataSet();

        // Create SqlDataAdapters for 2 tabels (parent and child)
        private SqlDataAdapter parentAdapter = new SqlDataAdapter();
        private SqlDataAdapter childAdapter = new SqlDataAdapter();

        // Create BindingSources for DataTables parent and child
        private BindingSource parentBS = new BindingSource(); 
        private BindingSource childBS = new BindingSource();

        public void createTextBoxesParent()
        {
            int margin = 15;
            int textBoxHeight = 20;
            int y = margin;
            for (int i = 0; i < nrColumnsParent; i++)
            {
                TextBox textBox = new TextBox();

                textBox.Name = "parentTextBox" + i;
                textBox.Size = new Size(parentPanel.Width, 20);
                textBox.Location = new Point(textBox.Location.X, y);
                y += textBoxHeight + margin;
                textBox.Text = parentColumnNamesList[textBoxesParent.Count];

                parentPanel.Controls.Add(textBox);
                textBoxesParent.Add(textBox);
            }
        }

        public void createTextBoxesChild()
        {
            int margin = 15;
            int textBoxHeight = 20;
            int y = margin;
            for (int i = 0; i < nrColumnsChild; i++)
            {
                TextBox textBox2 = new TextBox();

                textBox2.Name = "childTextBox" + i;
                textBox2.Size = new Size(childPanel.Width, 20);
                textBox2.Location = new Point(textBox2.Location.X, y);
                y += textBoxHeight + margin;
                textBox2.Text = childColumnNamesList[textBoxesChild.Count];

                childPanel.Controls.Add(textBox2);
                textBoxesChild.Add(textBox2);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refreshTables();
        }

        // Refresh the tables
        private void refreshTables()
        {
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    dataSet = new DataSet();

                    parentAdapter = new SqlDataAdapter();
                    childAdapter = new SqlDataAdapter();

                    parentBS = new BindingSource();
                    childBS = new BindingSource();

                    parentAdapter.SelectCommand = new SqlCommand(selectParentTable, connection);
                    childAdapter.SelectCommand = new SqlCommand(selectChildTable, connection);

                    parentAdapter.Fill(dataSet, parentTableName);
                    childAdapter.Fill(dataSet, childTableName);

                    parentBS.DataSource = dataSet.Tables[parentTableName];
                    dataGridViewParent.DataSource = parentBS;

                    DataColumn parentPK = dataSet.Tables[parentTableName].Columns[parentColumnNamesList[0]];
                    DataColumn childFK = dataSet.Tables[childTableName].Columns[childColumnNamesList[3]];
                    DataRelation relation = new DataRelation("fk_parent_child", parentPK, childFK);
                    dataSet.Relations.Add(relation);

                    childBS.DataSource = parentBS;
                    childBS.DataMember = "fk_parent_child";
                    dataGridViewChild.DataSource = childBS;

                    clearParentTextBox();
                    clearChildTextBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        // Reset values in Child TextBox
        private void clearChildTextBox()
        {
            // Write in TextBoxes the values of the tables
            int i = 0;
            foreach (TextBox textBox in textBoxesChild)
            {
                textBox.Text = childColumnNamesList[i];
                i++;
            }
        }

        // Reset values in Parent TextBox
        private void clearParentTextBox()
        {
            int i = 0;
            foreach (TextBox textBox in textBoxesParent)
            {
                textBox.Text = parentColumnNamesList[i];
                i++;
            };
        }

        private void dataGridViewParent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                clearParentTextBox();
                refreshTables();

                // Write in TextBoxes the values of the tables
                int i = 0;
                foreach (TextBox textBox in textBoxesParent)
                {
                    textBox.Text = dataGridViewParent.Rows[e.RowIndex].Cells[i].Value.ToString();
                    i++;
                };
            }
        }

        private void dataGridViewChild_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                clearChildTextBox();
                refreshTables();

                // Write in TextBoxes the values of the tables
                int i = 0;
                foreach (TextBox textBox in textBoxesChild)
                {
                    textBox.Text = dataGridViewChild.Rows[e.RowIndex].Cells[i].Value.ToString();
                    i++;
                }
            }
        }

        // Button click to show the flights
        private void showFlightsButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conection = new SqlConnection(connectionString))
                {

                    // Make the connection and populate the table
                    conection.Open();
                    string select = ConfigurationSettings.AppSettings["selectAllParent"];
                    childAdapter.SelectCommand = new SqlCommand(select, conection);
                    childAdapter.Fill(dataSet, parentTableName);
                    dataGridViewParent.DataSource = dataSet.Tables[parentTableName];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            // show the textboxes for each column
            createTextBoxesParent();
        }

        // Button click to show the companies
        private void showCompaniesButton_Click(object sender, EventArgs e)
        {

            try
            {
                using (SqlConnection conection = new SqlConnection(connectionString))
                {

                    // Make the connection and populate the table
                    conection.Open();
                    string select = ConfigurationSettings.AppSettings["selectAllChild"];
                    childAdapter.SelectCommand = new SqlCommand(select, conection);
                    childAdapter.Fill(dataSet, childTableName);
                    dataGridViewChild.DataSource = dataSet.Tables[childTableName];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            // show the textboxes for each column
            createTextBoxesChild();
        }

        // Button click to update a selected company
        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand updateCommand = new SqlCommand(ConfigurationManager.AppSettings["updateChild"], connection);

                    int i = 0;
                    foreach (string column in childColumnNamesParamList)
                    {
                        updateCommand.Parameters.AddWithValue(column, textBoxesChild[i].Text);
                        i++;
                    }

                    updateCommand.ExecuteNonQuery();
                    
                    refreshTables();

                    MessageBox.Show("Companie actualizata cu succes!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        // Button click to add a company when a flight is selected
        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand insertComand = new SqlCommand(ConfigurationManager.AppSettings["insertChild"], connection);

                    int i = 0;
                    foreach (string column in childColumnNamesParamList)
                    {
                        insertComand.Parameters.AddWithValue(column, textBoxesChild[i].Text);
                        i++;
                    }

                    insertComand.ExecuteNonQuery();

                    refreshTables();

                    MessageBox.Show("Companie adaugata cu succes!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        // Button click to delete a company when it's selected
        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand deleteCommand = new SqlCommand(ConfigurationManager.AppSettings["deleteChild"], connection);

                    deleteCommand.Parameters.AddWithValue(childColumnNamesParamList[0], textBoxesChild[0].Text);

                    deleteCommand.ExecuteNonQuery();

                    refreshTables();

                    MessageBox.Show("Companie stearsa cu succes!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }




        // Mouse hover events on buttons
        private void showZborButton_MouseHover(object sender, EventArgs e)
        {
            showZborButton.BackColor = Color.Azure;

        }

        private void showZborButton_MouseLeave(object sender, EventArgs e)
        {
            showZborButton.BackColor = Color.CornflowerBlue;

        }

        private void showCompanieButton_MouseHover(object sender, EventArgs e)
        {
            showCompanieButton.BackColor = Color.Azure;
        }

        private void showCompanieButton_MouseLeave(object sender, EventArgs e)
        {
            showCompanieButton.BackColor = Color.CornflowerBlue;
        }

        private void addButton_MouseHover(object sender, EventArgs e)
        {
            addButton.BackColor = Color.Azure;
        }

        private void addButton_MouseLeave(object sender, EventArgs e)
        {
            addButton.BackColor = Color.CornflowerBlue;
        }

        private void deleteButton_MouseHover(object sender, EventArgs e)
        {
            deleteButton.BackColor = Color.Azure;
        }

        private void deleteButton_MouseLeave(object sender, EventArgs e)
        {
            deleteButton.BackColor = Color.CornflowerBlue;
        }

        private void InitializeComponent()
        {
            dataGridViewParent = new DataGridView();
            dataGridViewChild = new DataGridView();
            label8 = new Label();
            showZborButton = new Button();
            showCompanieButton = new Button();
            label9 = new Label();
            label10 = new Label();
            addButton = new Button();
            deleteButton = new Button();
            label7 = new Label();
            updateButton = new Button();
            parentPanel = new Panel();
            childPanel = new Panel();
            ((ISupportInitialize)dataGridViewParent).BeginInit();
            ((ISupportInitialize)dataGridViewChild).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewParent
            // 
            dataGridViewParent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewParent.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewParent.BackgroundColor = Color.LightCyan;
            dataGridViewParent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewParent.GridColor = Color.CornflowerBlue;
            dataGridViewParent.Location = new Point(12, 49);
            dataGridViewParent.Name = "dataGridViewParent";
            dataGridViewParent.RowHeadersWidth = 51;
            dataGridViewParent.RowTemplate.Height = 29;
            dataGridViewParent.Size = new Size(460, 276);
            dataGridViewParent.TabIndex = 12;
            dataGridViewParent.CellClick += dataGridViewParent_CellClick;
            // 
            // dataGridViewChild
            // 
            dataGridViewChild.BackgroundColor = Color.LightCyan;
            dataGridViewChild.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewChild.GridColor = Color.CornflowerBlue;
            dataGridViewChild.Location = new Point(12, 410);
            dataGridViewChild.Name = "dataGridViewChild";
            dataGridViewChild.RowHeadersWidth = 51;
            dataGridViewChild.RowTemplate.Height = 29;
            dataGridViewChild.Size = new Size(460, 312);
            dataGridViewChild.TabIndex = 13;
            dataGridViewChild.CellClick += dataGridViewChild_CellClick;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Baskerville Old Face", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            label8.Location = new Point(901, 328);
            label8.Name = "label8";
            label8.Size = new Size(134, 26);
            label8.TabIndex = 15;
            label8.Text = "Tabel Copil";
            // 
            // showZborButton
            // 
            showZborButton.BackColor = Color.CornflowerBlue;
            showZborButton.Cursor = Cursors.Hand;
            showZborButton.FlatAppearance.MouseOverBackColor = Color.White;
            showZborButton.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point);
            showZborButton.Location = new Point(511, 130);
            showZborButton.Name = "showZborButton";
            showZborButton.Size = new Size(193, 86);
            showZborButton.TabIndex = 16;
            showZborButton.Text = "Afiseaza toate inregistrarile pentru tabela parinte";
            showZborButton.UseVisualStyleBackColor = false;
            showZborButton.Click += showFlightsButton_Click;
            showZborButton.MouseLeave += showZborButton_MouseLeave;
            showZborButton.MouseHover += showZborButton_MouseHover;
            // 
            // showCompanieButton
            // 
            showCompanieButton.BackColor = Color.CornflowerBlue;
            showCompanieButton.Cursor = Cursors.Hand;
            showCompanieButton.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point);
            showCompanieButton.Location = new Point(511, 462);
            showCompanieButton.Name = "showCompanieButton";
            showCompanieButton.Size = new Size(181, 87);
            showCompanieButton.TabIndex = 17;
            showCompanieButton.Text = "Afiseaza toate inregistrarile pentru tabela copil";
            showCompanieButton.UseVisualStyleBackColor = false;
            showCompanieButton.Click += showCompaniesButton_Click;
            showCompanieButton.MouseLeave += showCompanieButton_MouseLeave;
            showCompanieButton.MouseHover += showCompanieButton_MouseHover;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.LightSkyBlue;
            label9.Font = new Font("Baskerville Old Face", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            label9.ForeColor = Color.Black;
            label9.Location = new Point(173, 9);
            label9.Name = "label9";
            label9.Size = new Size(150, 26);
            label9.TabIndex = 18;
            label9.Text = "Tabel Parinte";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.LightSkyBlue;
            label10.Font = new Font("Baskerville Old Face", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            label10.Location = new Point(189, 366);
            label10.Name = "label10";
            label10.Size = new Size(134, 26);
            label10.TabIndex = 19;
            label10.Text = "Tabel Copil";
            // 
            // addButton
            // 
            addButton.BackColor = Color.CornflowerBlue;
            addButton.Cursor = Cursors.Hand;
            addButton.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point);
            addButton.Location = new Point(686, 641);
            addButton.Name = "addButton";
            addButton.Size = new Size(157, 81);
            addButton.TabIndex = 21;
            addButton.Text = "Adaugare";
            addButton.UseVisualStyleBackColor = false;
            addButton.Click += addButton_Click;
            addButton.MouseLeave += addButton_MouseLeave;
            addButton.MouseHover += addButton_MouseHover;
            // 
            // deleteButton
            // 
            deleteButton.BackColor = Color.CornflowerBlue;
            deleteButton.Cursor = Cursors.Hand;
            deleteButton.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point);
            deleteButton.Location = new Point(1038, 641);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(153, 81);
            deleteButton.TabIndex = 22;
            deleteButton.Text = "Stergere";
            deleteButton.UseVisualStyleBackColor = false;
            deleteButton.Click += deleteButton_Click;
            deleteButton.MouseLeave += deleteButton_MouseLeave;
            deleteButton.MouseHover += deleteButton_MouseHover;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Baskerville Old Face", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(885, 29);
            label7.Name = "label7";
            label7.Size = new Size(150, 26);
            label7.TabIndex = 14;
            label7.Text = "Tabel Parinte";
            // 
            // updateButton
            // 
            updateButton.BackColor = Color.CornflowerBlue;
            updateButton.Cursor = Cursors.Hand;
            updateButton.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point);
            updateButton.Location = new Point(861, 641);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(157, 81);
            updateButton.TabIndex = 29;
            updateButton.Text = "Actualizare";
            updateButton.UseVisualStyleBackColor = false;
            updateButton.Click += updateButton_Click;
            // 
            // parentPanel
            // 
            parentPanel.Location = new Point(745, 58);
            parentPanel.Name = "parentPanel";
            parentPanel.Size = new Size(416, 243);
            parentPanel.TabIndex = 30;
            // 
            // childPanel
            // 
            childPanel.Location = new Point(745, 366);
            childPanel.Name = "childPanel";
            childPanel.Size = new Size(416, 252);
            childPanel.TabIndex = 31;
            // 
            // Form1
            // 
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1213, 758);
            Controls.Add(childPanel);
            Controls.Add(parentPanel);
            Controls.Add(updateButton);
            Controls.Add(addButton);
            Controls.Add(deleteButton);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(showCompanieButton);
            Controls.Add(showZborButton);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(dataGridViewChild);
            Controls.Add(dataGridViewParent);
            Name = "Form1";
            ((ISupportInitialize)dataGridViewParent).EndInit();
            ((ISupportInitialize)dataGridViewChild).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}