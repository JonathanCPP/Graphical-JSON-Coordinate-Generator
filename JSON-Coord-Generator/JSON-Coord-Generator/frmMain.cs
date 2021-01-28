using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace JSON_Coord_Generator
{

    public partial class frmMenu : Form
    {
        public static int DefinitiveCoordX;     //used to make the right number of squares      
        public static int DefinitiveCoordY;     // "" ""
        public static int DefinitiveCoordZ;     // "" ""
        public static string name;              //filename
        byte bytCoordsGood;                     //decide whether to start program or not based on conditions passes

        public frmMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// check if the value in XYZ textboxes are good and start program if good
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txbX.Text, out int xcord) && xcord > 0)
            {
                bytCoordsGood++;
            }
            if (int.TryParse(txbY.Text, out int ycord) && ycord > 0)
            {
                bytCoordsGood++;
            }
            if (int.TryParse(txbZ.Text, out int zcord) && zcord > 0)
            {
                bytCoordsGood++;
            }
            if (bytCoordsGood == 3)
            {
                name = textBox2.Text;
                btnStart.Text = "Loading...";
                DefinitiveCoordX = xcord;
                DefinitiveCoordY = ycord;
                DefinitiveCoordZ = zcord;
                Form frmStartGrid = new frmGrid();
                this.Hide();
                frmStartGrid.Show();
            }
            else
            {
                bytCoordsGood = 0;
            }
        }
    }
}
