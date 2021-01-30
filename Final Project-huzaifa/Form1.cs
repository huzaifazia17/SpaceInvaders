using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Drawing.Text;
using AudioPlayer;



namespace Final_Project_huzaifa
{
    public partial class Form1 : Form
    {
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        //                                                               DECLARING ALL VARIABLES
        //---------------------------------------------------------------------------------------------------------------------------------------------------------

        // declares variable for start button
        Button startBtn = new Button();
        // declares variable for exit button
        Button exitBtn = new Button();
        // declares variable for Help utton
        Button helpBtn = new Button();
        //Creates the rectangle for the player
        Rectangle player;
        //declares list rectangle for enemy
        List<Rectangle> enemyRectList;
        //declares list rectangle for bullet
        List<Rectangle> bulletRect;
        //declares the variable eHealth as a list
        List<int> eHealth;
        //declares the variable enemyHealth
        int enemyHealth = 1;
        // declares dx as an integer which is used for the movement of the enemy and bullet
        int dx;
        // declares dy as an integer which is used for the movement of the enemy and bullet
        int dy;
        //declares the variable x used for the position of the bullet
        int x;
        //declares the variable y used for the position of the bullet
        int y;
        // declares integer dxp used for movement of player
        int dxp;
        // declares integer dyp used for movement of player
        int dyp;
        // constant variable for screen width
        const int screenWidth = 770;
        // sets constant variable for screen Height
        const int screenHeight = 700;
        //declares the variable ey
        int ey = 1;
        // declares image variable
        Image image;
        // declares image variable
        Image image2;
        // declares image variable
        Image image4;
        // declares background image variable
        Image mainBackground;
        // sets random number
        Random randomNum = new Random();
        // stes a bool variable of selected to be false
        bool selected = false;
        //sets integer variable for high score
        int highscore;
        //sets integer variable for score
        int score;
        // declares font collection
        PrivateFontCollection fontCollection;
        // declares main font and game font
        Font mainFont, gameFont;
        //craetes timer
        Timer refresh = new Timer();
        //declares  audio file player for shoot sound
        AudioFilePlayer shootSound;
        //declares  audio file player for background sound
        AudioFilePlayer backMusic;

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------

        public Form1()
        {
            //INITIALIZES THE COMPONENTS OF THE FORM
            InitializeComponent();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        //                                                                    FORM LOAD
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            // sets background image of start page
            BackgroundImage = Image.FromFile(Application.StartupPath + @"\mainBack.jpg", true);

            // creates event handler for start button
            startBtn.Click += StartBtn_Click;
            //sets width of start button
            startBtn.Width = 500;
            //sets height of start button
            startBtn.Height = 50;
            //creates the start button
            this.Controls.Add(startBtn);
            //changes colour of start button
            startBtn.BackColor = Color.Black;

            // creates event handler for exit button
            exitBtn.Click += ExitBtn_Click;
            //sets text of exit button
            exitBtn.Text = "Exit";
            //sets width of exit button
            exitBtn.Width = 150;
            //sets height of exit button
            exitBtn.Height = 100;
            //brings exit button to front
            exitBtn.BringToFront();
            //creates the exit button
            this.Controls.Add(exitBtn);
            //changes colour of exit button
            exitBtn.BackColor = Color.Black;

            // creates event handler for help button
            helpBtn.Click += HelpBtn_Click;
            //sets text of help button
            helpBtn.Text = "Instructions";
            //sets width of help button
            helpBtn.Width = 160;
            //sets height of help button
            helpBtn.Height = 100;
            //brings help button to front
            helpBtn.BringToFront();
            //creates the help button
            this.Controls.Add(helpBtn);
            //changes colour of help button
            helpBtn.BackColor = Color.Black;

            //sets location for the start button
            startBtn.Location = new Point((this.ClientSize.Width / 2 - startBtn.Width / 2 + 240), (this.ClientSize.Height / 2 - startBtn.Height + 250));
            //sets location for the help button
            helpBtn.Location = new Point(590, 560);
            //sets location for the exit Button
            exitBtn.Location = new Point(0, 560);

            // stop screen from blinking
            this.DoubleBuffered = true;
            //change screen height
            this.Height = screenHeight;
            //change screen width
            this.Width = screenWidth;
            //change text of form
            this.Text = "Galaxy Invaders";
            // change start position of form
            this.StartPosition = FormStartPosition.CenterScreen;
            // stop user from maximizing form
            this.MaximizeBox = false;

            //creates event handler for paint
            this.Paint += Form1_Paint;

            //constructs the List for bullets
            bulletRect = new List<Rectangle>();
            //constructs the List for enemies
            enemyRectList = new List<Rectangle>();
            //constructs the List for enemy health
            eHealth = new List<int>();

            //creates font collection
            fontCollection = new PrivateFontCollection();
            //gets font from file
            fontCollection.AddFontFile(Application.StartupPath + @"\square.ttf");
            //gets font from file
            fontCollection.AddFontFile(Application.StartupPath + @"\Android 101.ttf");

            //set main font to fontcollection and set its family and size
            mainFont = new Font(fontCollection.Families[1], 40);
            //set game font to fontcollection and set its family and size
            gameFont = new Font(fontCollection.Families[0], 14);

            //gets image from file for player
            image = Image.FromFile(Application.StartupPath + @"\player.png", true);
            //gets image from file for bullet
            image4 = Image.FromFile(Application.StartupPath + @"\bullet.png", true);
            //gets image from file for enemy
            image2 = Image.FromFile(Application.StartupPath + @"\enemy.png", true);
            //gets image from file for background
            mainBackground = Image.FromFile(Application.StartupPath + @"\mainBack.jpg", true);

            //creates event handler for mouse up
            this.MouseUp += Form1_MouseUp;
            //creates event handler for mouse move
            this.MouseMove += Form1_MouseMove;
            //creates event handler for mouse click
            this.MouseClick += Form1_MouseClick;
            //creates event handler for mouse down
            this.MouseDown += Form1_MouseDown;


            //creates the interval for the refresh timer
            refresh.Tick += Refresh_Tick;
            //sets the interval for the timer
            refresh.Interval = 5;
            //starts the timer
            refresh.Start();

            //checks to see if the file exists
            if (File.Exists(Application.StartupPath + @"\highscore.txt"))
            {
                //reads what is written in the file
                StreamReader hScore = new StreamReader(Application.StartupPath + @"\highscore.txt");

                //converts the number from the file into an integer and uses that as variable for highscore
                highscore = Convert.ToInt32(hScore.ReadLine());

                //closes the file
                hScore.Close();
            }

            //variable sets as audioplayer
            shootSound = new AudioFilePlayer();
            //gets sound from file
            shootSound.setAudioFile(Application.StartupPath + @"\shoot.mp3");

            //variable sets as audioplayer
            backMusic = new AudioFilePlayer();
            //gets sound from file
            backMusic.setAudioFile(Application.StartupPath + @"\backMusic.mp3");
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------------------------------
        //                                                     EVENT HADNLER FOR THE TIMER
        //-------------------------------------------------------------------------------------------------------------------------------------------

        private void Refresh_Tick(object sender, EventArgs e)
        {
            //refreshes the screen
            this.Invalidate();

            //differentiates between the player and bullet----------------------------

            //for loop runs to check the list
            for (int i = 0; i < bulletRect.Count; i++)
            {
                //changes the location of bullet
                bulletRect[i] = new Rectangle(bulletRect[i].X, bulletRect[i].Y + dy, bulletRect[i].Width, bulletRect[i].Height);

                //removes bullet when it reaches the top of the screen
                if (bulletRect[i].Top <= this.Top)
                {
                    //removes from the list
                    bulletRect.RemoveAt(i);
                    //removes the bullet
                    --i;
                }
            }

            //-----------------------------------------------------------------------------

            //for loop runs to check the list
            for (int i = 0; i < enemyRectList.Count; i++)
            {
                //changes the location of the enemy
                enemyRectList[i] = new Rectangle(enemyRectList[i].X, enemyRectList[i].Y + ey, enemyRectList[i].Width, enemyRectList[i].Height);
            }

            //runs to check if any of the enemies hit the player
            for (int i = 0; i < enemyRectList.Count; ++i)
            {
                //if the player has hit the enemy than this will run
                if (enemyRectList[i].IntersectsWith(player))
                {
                    //stops timer
                    refresh.Stop();

                    //shows game over
                    MessageBox.Show("Game Over");

                    //closes the form
                    Application.Exit();

                    //runs method
                    hiscore();

                    //breaks the loop
                    break;
                }
            }

            //for loop runs to check the list for if the enemy hits the bullet
            for (int i = 0; i < enemyRectList.Count; i++)
            {
                //runs to check to see if it hits the enemy
                for (int j = 0; j < bulletRect.Count; j++)
                {


                    //if bullet hits enemy this will run
                    if (bulletRect[j].IntersectsWith(enemyRectList[i]))
                    {
                        //removes from the list
                        bulletRect.RemoveAt(j);

                        //minuses 1 each time it runs
                        eHealth[i]--;

                        //if the enemies health is less than or equal to 0 than this will run
                        if (eHealth[i] <= 0)
                        {
                            //removes from the list
                            enemyRectList.RemoveAt(i);

                            //will remove enemy health 
                            eHealth.RemoveAt(i);

                            //increases the score by 10 everytime an enemy is killed
                            score += 10;

                            //takes out enemy if it is hit
                            --i;
                            //breaks the loop
                            break;
                        }


                    }

                }
            }

            //if enemies are gone then this will run
            if (enemyRectList.Count == 0)
            {
                //runs method for spawning enemies
                spawnEnemies();

            }


            //runs for loop to check number of enemies
            for (int i = 0; i < enemyRectList.Count; i++)
            {
                //if the enemies hit the bottom of the screen then this will run
                if (enemyRectList[i].Bottom >= this.ClientSize.Height)
                {
                    //stops timer
                    refresh.Stop();

                    //shows game over
                    MessageBox.Show("Game Over!");

                    //closes the form
                    Application.Exit();

                    //runs method
                    hiscore();

                    //breaks the loop
                    break;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------------------------------
        //                                      EVENT HANDLERS FOR THE MOVEMENT OF THE PLAYER VIA MOUSE OR KEYPAD
        //-------------------------------------------------------------------------------------------------------------------------------------------

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // runs to check if mouse is selected
            selected = false;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //moves the rectangle up 5 pixels
            dy = -15;

            //adds a rectangle from the list
            bulletRect.Add(new Rectangle(player.Left + player.Width / 2, player.Top, 5, 5));

            //plays shooting sound
            shootSound.play();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // if the mouse is selected then
            if (selected == true)
            {
                //the players location will be the location of the mouse
                player.Location = e.Location;
            }

            //create a temporary rectangle
            Rectangle tempRect;
            //makes the temporary rectangle equal to the player
            tempRect = player;
            // the X position of the temp rec will equal to the x position of the mouse
            tempRect.X = e.X;
            // if the right side of the temp rec is greater than the screen width then
            if (tempRect.Right > this.ClientSize.Width)
            {
                //this will change the position of the temp rec so it moves back in the screen
                tempRect.X = this.ClientSize.Width - tempRect.Width;
            }
            //the player will now equal to the temporary rectangle
            player = tempRect;

            // create another temporary rectangle
            Rectangle tempRectTwo;
            //makes the temporary rectangle equal to the player
            tempRectTwo = player;
            // the Y position of the temp rec will equal to the Y position of the mouse
            tempRectTwo.Y = e.Y;
            // if the bottom side of the temp rec is greater than the screen height then
            if (tempRectTwo.Bottom > this.ClientSize.Height)
            {
                //this will change the position of the temp rec so it moves back in the screen
                tempRectTwo.Y = this.ClientSize.Height - tempRectTwo.Height;
            }
            //the player will now equal to the second temporary rectangle
            player = tempRectTwo;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            // if mouse is not selected then the player will not move
            selected = true;
            // this will stop the player from moving its y positon
            dyp = 0;
            // this will stop the player from moving its X positon
            dxp = 0;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------------------------------
        //                                          EVENT HANDLERS FOR THE START, HELP, AND EXIT BUTTONS
        //-------------------------------------------------------------------------------------------------------------------------------------------

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            // hide the help button when the help button is clicked
            helpBtn.Hide();
            // hide the exit button when the help button is clicked
            exitBtn.Hide();
            //change the location fo the start button
            startBtn.Location = new Point(140, 500);
            //change the background image
            BackgroundImage = Image.FromFile(Application.StartupPath + @"\inBack.jpg", true);
            //stop the timer
            refresh.Stop();
            //stops the shooting sound
            shootSound.stop();

        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            //run the game end method
            gameEnd();
        }


        private void StartBtn_Click(object sender, EventArgs e)
        {
            // will allow the background music to contunially play
            backMusic.playLooping();
            //will start the timer
            refresh.Start();
            // hide the help button when the start button is clicked
            helpBtn.Hide();
            // hide the exit button when the start button is clicked
            exitBtn.Hide();
            // hide the start button when the start button is clicked
            startBtn.Hide();
            //change the background image
            BackgroundImage = Image.FromFile(Application.StartupPath + @"\background.jpg", true);
            // set the coordinates for player
            player = new Rectangle((this.ClientSize.Width / 2 - 40), (this.ClientSize.Height - 100), 100, 100);

        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------------------------------
        //                                           METHOD FOR QUITTING THE GAME THROUGH THE EXIT BUTTON
        //-------------------------------------------------------------------------------------------------------------------------------------------

        private void gameEnd()
        {
            // show messsage that the player has chosen to exit the game
            MessageBox.Show("You have chosen to exit the appplication");
            //close the form
            Application.Exit();
            //stop playing the background music
            backMusic.stop();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------------------------------
        //                                                     METHOD FOR INPUTTING HIGHSCORE
        //-------------------------------------------------------------------------------------------------------------------------------------------

        private void hiscore()
        {
            //stops timer
            refresh.Stop();

            //rewrites the file that is in the debug folder
            StreamWriter hSwriter = new StreamWriter(Application.StartupPath + @"\highscore.txt", false);

            //if the score is higher than the highscore than this will run
            if (score > highscore)
            {
                //rewrites the highscore into the score
                hSwriter.WriteLine(score);
            }
            //else
            else
            {
                //sets the highscore to what it originally was
                hSwriter.WriteLine(highscore);
            }

            //closes the file
            hSwriter.Close();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------------------------------
        //                                                     METHOD FOR RE-SPAWNING ENEMIES
        //-------------------------------------------------------------------------------------------------------------------------------------------

        private void spawnEnemies()
        {
            //for loop runs to spawn 24 enemies
            for (int i = 0; i < 24; i++)
            {

                //if i equals to 0 than this will run
                if (i == 0)
                {
                    //adds an enemy to the list
                    enemyRectList.Add(new Rectangle(40, 40, 40, 40));

                    //sets enemyhealth to the variable
                    eHealth.Add(enemyHealth);

                    //adds another enemy
                    i++;
                }

                //if i is modulus divided by 12 and equals to 0 than this will run
                if (i % 12 == 0)
                {
                    //adds more enemies to the list but in another row
                    enemyRectList.Add(new Rectangle(enemyRectList[i - 12].X, enemyRectList[i - 12].Bottom + 20, 40, 40));

                    //sets enemyhealth to the variable
                    eHealth.Add(enemyHealth);
                }

                //else
                else
                {
                    //adds enemies to the list but beside other enemies
                    enemyRectList.Add(new Rectangle(enemyRectList[enemyRectList.Count - 1].Right + 20, enemyRectList[enemyRectList.Count - 1].Y, 40, 40));

                    //sets enemyhealth to the variable
                    eHealth.Add(enemyHealth);
                }
            }

            //adds 1 to enemyhealth
            enemyHealth++;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------


        //----------------------------------------------------------------------------------------------------------------------------------------------------
        //                                            DRAWS ALL THE IMAGES, RECATANGLES AND WORDS ONTO THE SCREEN
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // if the start button is not visible than
            if (startBtn.Visible == false)
            {
                // draw string for the score and set its position,size, colour and font
                e.Graphics.DrawString("Score: " + score, gameFont, Brushes.White, new Rectangle(0, 0, 150, 150));

                // draw string for the highscore and set its position,size, colour and font
                e.Graphics.DrawString("HighScore: " + highscore, gameFont, Brushes.White, new Rectangle(200, 0, 200, 200));

                // draw the image inside the rectangle
                e.Graphics.DrawImage(image, player);

                //runs to check the list
                for (int i = 0; i < bulletRect.Count; ++i)
                {
                    //fills the bullet with a red color
                    e.Graphics.FillRectangle(Brushes.Red, bulletRect[i]);
                }

                //runs to check the list
                for (int i = 0; i < enemyRectList.Count; ++i)
                {
                    //drwas an image in the rectangle for the enemy
                    e.Graphics.DrawImage(image2, enemyRectList[i]);
                }
            }
            // otherwise if the start button is visible and the help button is not visible than
            else if (startBtn.Visible == true && helpBtn.Visible == false)
            {
                // draw string for the start button and set its position,size, colour and font
                e.Graphics.DrawString("START", mainFont, Brushes.BlueViolet, new Rectangle(280, 450, this.ClientSize.Width, this.ClientSize.Height));
                // draw string for the instructions and set its position,size, colour and font. INSTRUCTIONS ARE ALSO IN THE MANUAL IN EXTERNAL COMMENTS
                e.Graphics.DrawString("\n                                Instructions\n\n- Use the mouse to control players movement\n\n- Click to shoot enemies\n\n- If player is hit by enemies then you lose\n\n- you can move wherever you want\n\n- each wave of enemies are stronger\n\n- When enemies reach the bottom of the screen, you lose\n\n- Player has to beat current highscore\n\n                                GOOD LUCK!", gameFont, Brushes.White, new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height));

            }
            //otherwise
            else
            {
                // draw string for the title-galaxy invaders- and set its position,size, colour and font
                e.Graphics.DrawString("GALAXY INVADERS", mainFont, Brushes.BlueViolet, new Rectangle(90, 20, this.ClientSize.Width, this.ClientSize.Height));
                // draw string for the start button and set its position,size, colour and font
                e.Graphics.DrawString("START", mainFont, Brushes.BlueViolet, new Rectangle(280, 280, this.ClientSize.Width, this.ClientSize.Height));
                // draw string for the help button and set its position,size, colour and font
                e.Graphics.DrawString("HELP", mainFont, Brushes.BlueViolet, new Rectangle(580, 515, this.ClientSize.Width, this.ClientSize.Height));
                // draw string for the exit button and set its position,size, colour and font
                e.Graphics.DrawString("EXIT", mainFont, Brushes.BlueViolet, new Rectangle(0, 515, this.ClientSize.Width, this.ClientSize.Height));
            }

            // draw string for the credits and set its position,size, colour and font
            e.Graphics.DrawString("Developed By: Huzaifa Zia", gameFont, Brushes.Blue, new Rectangle(225, 640, this.ClientSize.Width, this.ClientSize.Height));

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------------
    }
}

