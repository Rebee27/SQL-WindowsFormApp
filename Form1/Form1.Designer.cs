namespace Lab1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        private System.Windows.Forms.DataGridView dataGridViewParent;
        private System.Windows.Forms.DataGridView dataGridViewChild;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button showZborButton;
        private System.Windows.Forms.Button showCompanieButton;
        private Label label9;
        private Label label10;
        private Button addButton;
        private Button deleteButton;
        private Label label7;
        private Button updateButton;
        private Panel parentPanel;
        private Panel childPanel;
        private List<TextBox> textBoxesChild = new List<TextBox>();
        private List<TextBox> textBoxesParent = new List<TextBox>();
    }
}