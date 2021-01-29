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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JSON_Coord_Generator
{
    public partial class frmGrid : Form
    {
        const byte bytSquareSize = 90;      //size of square

        int posX;                           //horizontal position of square
        int posY;                           //vertical position of square
        Point TopLeft;
        Point BottomRight;
        string FinalData;                   //what you write to file    
        byte CurrentSelected = 0;           //current layer
        bool commaput = false;              //Decide whether or not to put a comma in file
        bool hovermode = false;             //turn on hover to place blocks
        int BlockCounter;                   //tell how many blocks you need for the building
        Button[,,] GridSquare;              //array system for buttons
        Form Help = new About();            //generate help window

        public frmGrid()
        {
            InitializeComponent();
            BuildGrid();
            DisplayGrid();
            TopLeft = new Point(bytSquareSize, bytSquareSize);
            BottomRight = new Point(pnlGrid.Width - bytSquareSize, pnlGrid.Height - bytSquareSize);
        }

        /// <summary>
        /// ferme tout le programme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseApp(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Decide properties for every single square and position in panel
        /// </summary>
        private void BuildGrid()
        {
            GridSquare = new Button[frmMenu.DefinitiveCoordX, frmMenu.DefinitiveCoordY, frmMenu.DefinitiveCoordZ];

            for (int y = 0; y < frmMenu.DefinitiveCoordY; y++)
            {
                for (int z = 0; z < frmMenu.DefinitiveCoordZ; z++)
                {
                    for (int x = 0; x < frmMenu.DefinitiveCoordX; x++)
                    {
                        GridSquare[x, y, z] = new Button();
                        GridSquare[x, y, z].Size = new Size(bytSquareSize, bytSquareSize);
                        GridSquare[x, y, z].BackColor = Color.Black;
                        GridSquare[x, y, z].ForeColor = Color.Gray;
                        GridSquare[x, y, z].Font = new Font("Myanmar Text", 11, FontStyle.Bold);
                        GridSquare[x, y, z].FlatStyle = FlatStyle.Flat;
                        GridSquare[x, y, z].BackgroundImageLayout = ImageLayout.Stretch;
                        GridSquare[x, y, z].MouseDown += new MouseEventHandler(PlaceBlock);
                        GridSquare[x, y, z].MouseMove += new MouseEventHandler(DragPlace);
                        GridSquare[x, y, z].Location = new Point(posX, posY);
                        GridSquare[x, y, z].Text = x + ", " + z;
                        GridSquare[x, y, z].Name = x + "/" + y + "/" + z;
                        GridSquare[x, y, z].Tag = 0;
                        GridSquare[x, y, z].Hide();
                        pnlGrid.Controls.Add(GridSquare[x, y, z]);

                        posX += bytSquareSize;
                    }
                    posX = 0;
                    posY += bytSquareSize;
                }
                posY = 0;
            }
        }

        /// <summary>
        /// give tag, colors and change counters when clicking on a square
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaceBlock(object sender, MouseEventArgs e)
        {
            Button clickedlabel = sender as Button;

            if (e.Button == MouseButtons.Left)
            {
                if (clickedlabel.Tag == null || clickedlabel.Tag.ToString() != "1")
                {
                    clickedlabel.BackColor = Color.Red;
                    clickedlabel.ForeColor = Color.White;
                    clickedlabel.Tag = 1;

                    BlockCounter++;
                    lblBlocks.Text = "Blocks : " + BlockCounter;
                    if (BlockCounter > 63)
                    {
                        lblStacks.Text = "Stacks : " + (BlockCounter / 64).ToString();
                    }
                    else
                    {
                        lblStacks.Text = "Stacks : 0";
                    }
                }
                else
                {
                    clickedlabel.BackColor = Color.Black;
                    clickedlabel.ForeColor = Color.Gray;
                    clickedlabel.Tag = 0;
                    BlockCounter--;
                    lblBlocks.Text = "Blocks : " + BlockCounter;
                    if(BlockCounter > 63)
                    {
                        lblStacks.Text = "Stacks : "+ (BlockCounter / 64).ToString();
                    }
                    else
                    {
                        lblStacks.Text = "Stacks : 0";
                    }
                }
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (hovermode != true)
                    {
                        hovermode = true;
                    }
                    else
                    {
                        hovermode = false;
                    }
                }
            }
        }

        /// <summary>
        /// hover mode to place blocks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragPlace(object sender, EventArgs e)
        {
            Button clickedlabel = sender as Button;
            clickedlabel.Select();
            if (hovermode == true)
            {
                if (Convert.ToInt32(clickedlabel.Tag) == 0)
                {
                    BlockCounter++;
                    lblBlocks.Text = "Blocks : " + BlockCounter;
                    if (BlockCounter > 63)
                    {
                        lblStacks.Text = "Stacks : " + (BlockCounter / 64).ToString();
                    }
                    else
                    {
                        lblStacks.Text = "Stacks : 0";
                    }
                }

                
                clickedlabel.BackColor = Color.Red;
                clickedlabel.ForeColor = Color.White;
                clickedlabel.Tag = 1;
            }
        }

        /// <summary>
        /// force bring to front on all slots
        /// </summary>
        private void DisplayGrid()
        {
            for (int y = 0; y < frmMenu.DefinitiveCoordY; y++)
            {
                for (int z = 0; z < frmMenu.DefinitiveCoordZ; z++)
                {
                    for (int x = 0; x < frmMenu.DefinitiveCoordX; x++)
                    {
                        GridSquare[x, 0, z].Show();
                    }
                }
            }
        }

        /// <summary>
        /// go down a layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (CurrentSelected > 0)
            {
                for (int z = 0; z < frmMenu.DefinitiveCoordZ; z++)
                {
                    for (int x = 0; x < frmMenu.DefinitiveCoordX; x++)
                    {
                        GridSquare[x, CurrentSelected, z].Hide();
                    }
                }


                CurrentSelected--;

                for (int z = 0; z < frmMenu.DefinitiveCoordZ; z++)
                {
                    for (int x = 0; x < frmMenu.DefinitiveCoordX; x++)
                    {
                        GridSquare[x, CurrentSelected, z].Show();
                    }
                }
                lblCouche.Text = "Layer " + CurrentSelected;
            }
        }

        /// <summary>
        /// go up a layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (CurrentSelected < frmMenu.DefinitiveCoordY - 1)
            {
                for (int z = 0; z < frmMenu.DefinitiveCoordZ; z++)
                {
                    for (int x = 0; x < frmMenu.DefinitiveCoordX; x++)
                    {
                        GridSquare[x, CurrentSelected, z].Hide();
                    }
                }

                CurrentSelected++;

                for (int z = 0; z < frmMenu.DefinitiveCoordZ; z++)
                {
                    for (int x = 0; x < frmMenu.DefinitiveCoordX; x++)
                    {
                        GridSquare[x, CurrentSelected, z].Show();
                    }
                }
            }
            lblCouche.Text = "Layer " + CurrentSelected;
        }

        /// <summary>
        /// write block positions to file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            FinalData = "{\"blocks\": [";

            for (int y = 0; y < frmMenu.DefinitiveCoordY; y++)
            {
                for (int z = 0; z < frmMenu.DefinitiveCoordZ; z++)
                {
                    for (int x = 0; x < frmMenu.DefinitiveCoordX; x++)
                    {
                        if (GridSquare[x, y, z].Tag.ToString() == "1")
                        {
                            if (commaput == true)
                            {
                                FinalData += ",";
                            }

                            FinalData += "[" + (-x) + "," + (y) + "," + (-z) + "]";
                            commaput = true;
                        }
                    }
                }
            }

            FinalData.Remove(FinalData.Length - 1, 1);
            FinalData += "]}";

            TextWriter TW = new StreamWriter(frmMenu.name + ".json");
            TW.WriteLine(FinalData);
            TW.Close();
            MessageBox.Show("Successfully saved \""+frmMenu.name+"\" in the same folder as the current .exe file.", "Success !");
            commaput = false;
        }

        /// <summary>
        /// show the help window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblHelp_Click(object sender, EventArgs e)
        {
                Help.ShowDialog();
        }
    }
}
