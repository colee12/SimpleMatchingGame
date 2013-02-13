using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        Random random = new Random();

        System.Media.SoundPlayer matchSound = new System.Media.SoundPlayer(@"C:\Windows\Media\chimes.wav");
        System.Media.SoundPlayer noMatchSound = new System.Media.SoundPlayer(@"C:\Windows\Media\chord.wav");
        System.Media.SoundPlayer successSound = new System.Media.SoundPlayer(@"C:\Windows\Media\tada.wav");


        // These keep track of the two labels
        // selected for comparison.
        Label firstClicked = null;
        Label secondClicked = null;

        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };
        
        public Form1()
        {
            InitializeComponent();

            AssignIconsToSquares();
        }

        private void AssignIconsToSquares()
        {
            // The TableLayoutPanel has 16 labels, 
            // and the icon list has 16 icons, 
            // so an icon is pulled at random from the list 
            // and added to each label 
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            // The timer is on after two non matching
            // icons have be revealed. Ignores clicks
            // if the timer is running.
            if (timer1.Enabled == true)
            {
                return;
            }

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // Check to see if it's already been revealed.5
                if (clickedLabel.ForeColor == Color.Black)
                {
                    return;
                }

                if (firstClicked == null)
                {
                    clickedLabel.ForeColor = Color.Black;
                    firstClicked = clickedLabel;

                    return;
                }

                // If the player gets this far, the timer isn't 
                // running and firstClicked isn't null, 
                // so this must be the second icon the player clicked 
                // Set its color to black
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                // Check to see if the player has won.
                CheckForWinner();

                // If the two icons match, keep them shown
                // and reset the selected icons to null.
                if (firstClicked.Text == secondClicked.Text)
                {
                    matchSound.Play();
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // If the player gets this far, the player  
                // clicked two different icons, so start the  
                // timer (which will wait three quarters of  
                // a second, and then hide the icons)
                noMatchSound.Play();
                timer1.Start();
            }
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                    {
                        return;
                    }
                }
            }

            // If the loop didn’t return, it didn't find 
            // any unmatched icons 
            // That means the user won. Show a message and close the form
            successSound.Play();
            MessageBox.Show("You matched all the icons!", "Congratulations");
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            timer1.Stop();

            // Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Reset firstClicked and secondClicked  
            // so the next time a label is 
            // clicked, the program knows it's the first click
            firstClicked = null;
            secondClicked = null;
        }
    }
}
