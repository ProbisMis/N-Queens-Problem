using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }


        List<int[]> bestheuristics; //for annealing
        
        
        //int[] randomPos;
        int numOfIterations;

        //constants for board
        const int tileSize = 40;
        public int gridSize = 8;
      
        // class member array of Panels to track chessboard tiles
        Panel[,] _chessBoardPanels;
        //queen locations
        int[] queenPositions;
        //queen pictures
        PictureBox[] PictureBoxes;
        //size
        int k;
        //for annealing best states
        int counter = 0;

        static readonly Random rand = new Random();


        public int[] randomQueen() {
        
             int[] result = new int[k];

            Random g = new Random();
            for (int i = 0; i < k; i++)
            {
                result[i] = g.Next(k);
            }

            return result;
        }

        //Function to get a random number 
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(max);
            }
        }
      
        public void drawQueens()
        {
            for (var n = 0; n < k; n++)
            {
                var picture = new PictureBox
                {
                    Size = new Size(40, 40),
                    Location = new Point(30 + tileSize * n, 30 + queenPositions[n] * 40),
                    Image = Properties.Resources.chess_151541_960_720,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    


                };
                PictureBoxes[n] = picture; //new pictures
                
                Controls.Add(picture);

            }
        }

        public void drawBoard() {
            
            var clr1 = Color.DarkGray;
            var clr2 = Color.White;
            
            for (var n = 0; n < gridSize; n++) {
                for (var m = 0; m < gridSize; m++) {
                    
                    // create new Panel control which will be one 
                    // chess board tile
                    var newPanel = new Panel
                    {
                        Size = new Size(tileSize, tileSize),
                        Location = new Point(30 + tileSize * n, 30 + tileSize * m)
                    };

                    

                    // add to Form's Controls so that they show up
                    Controls.Add(newPanel);

                    // add to our 2d array of panels for future use
                    _chessBoardPanels[n, m] = newPanel;

                    // color the backgrounds
                    if (n % 2 == 0)
                        newPanel.BackColor = m % 2 != 0 ? clr1 : clr2;
                    else
                        newPanel.BackColor = m % 2 != 0 ? clr2 : clr1;


                }
            
            }

        }
       
        private void deleteBoard()
        {

            for (var n = 0; n < gridSize; n++)
            {
                Controls.Remove(PictureBoxes[n]);
                for (var m = 0; m < gridSize; m++)
                {
                    Controls.Remove(_chessBoardPanels[n, m]);
                   
                }
            }
        }

        // event handler of Form Load... init things here
        private void Form_Load(object sender, EventArgs e, int k)
        {
            // initialize the "chess board"
            _chessBoardPanels = new Panel[k, k];
            PictureBoxes = new PictureBox[k];
            queenPositions = new int[k];
            gridSize = k; //NxN
            numOfIterations = Convert.ToInt32(textBox11.Text);
            bestheuristics = new List<int[]>();

            queenPositions = randomQueen();
            drawQueens();
            drawBoard();
            
        }
       
        //DELETE AND GENERATE 
        public void Form_InLoad(int k) {

            deleteBoard();


            drawQueens();
            drawBoard();
            
}

        private void button1_Click(object sender, EventArgs e)
        {
                      Form_Load(sender, e, k);
                     

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.Text == "4x4"){
                
                k=4;
                //Form_Load(sender, e, k);
               
            }
            else if (comboBox1.Text == "5x5"){
                k=5;
               // Form_Load(sender, e, k);
                
            }
               
            else if (comboBox1.Text == "6x6"){
                k=6;
               // Form_Load(sender, e, k);
            }
              
            else if (comboBox1.Text == "7x7"){
            k=7;
        //    Form_Load(sender, e, k);
            }
               
            else if (comboBox1.Text == "8x8"){
                k=8;
            //    Form_Load(sender, e, k);
            }
                
         
        }

        //GROUP BOX / ENABLE/ DISABLE

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = true;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
        }


        //SOLVE BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
           
            if (radioButton1.Checked == true)
            { 
               
                textBox9.Text = Convert.ToString(h(queenPositions));
                hillclimbing(numOfIterations);
                textBox10.Text = Convert.ToString(h(queenPositions));
                Form_InLoad(k);
            }
            else if (radioButton2.Checked == true)
            {
                
                textBox9.Text = Convert.ToString(h(queenPositions));
                annealing(numOfIterations, Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox1.Text));
                Form_InLoad(k);
                textBox10.Text = Convert.ToString(h(queenPositions));

            }
            else if (radioButton3.Checked == true)
            {
                
                textBox9.Text = Convert.ToString(h(queenPositions));
                kbeamsearch(Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox11.Text));
                Form_InLoad(k);
                textBox10.Text = Convert.ToString(h(queenPositions));
            }
            else if (radioButton4.Checked == true)
            {
                textBox9.Text = Convert.ToString(h(queenPositions));
                geneticAlgorithm(Convert.ToInt32(textBox4.Text), Convert.ToDouble(textBox7.Text), Convert.ToDouble(textBox6.Text), Convert.ToDouble(textBox5.Text), Convert.ToInt32(textBox8.Text));
                Form_InLoad(k);
                textBox10.Text = Convert.ToString(h(queenPositions));
            }

        }


        //DELETE BUTTON
        private void button3_Click(object sender, EventArgs e)
        {
            deleteBoard();

        }


        //HEURISTIC FUNCTION
        public int h(int[] board) {
            int h = 0;

            for (int i = 0; i < board.Length; i++)
                for (int j = i + 1; j < board.Length; j++)
                    if (board[i] == board[j] || Math.Abs(board[i] - board[j]) == j - i)
                        h += 1;

            return h;
        }

       
        //HILL CLIMBING
        public int[] hillclimbing(int numOfIterations) {

            Random g = new Random();
            
            for (int j = 0; j < numOfIterations && h(queenPositions) != 0; j++) {
                
                
                
                int htobeat = h(queenPositions);
                int temph = 0;
                int[] copyboard;

                for (var n = 0; n < gridSize; n++) //for each queen
                {
                    List<int> mylist = new List<int>(k);
                    
                    for (var m = 0; m < gridSize; m++)
                    {
                        copyboard = queenPositions;
                        copyboard[n] = m;
                        temph = h(copyboard);

                        if (temph < htobeat)
                        {
                            mylist.Clear();
                            mylist.Add(m);
                            htobeat = temph;                       
                        }
                        else if(temph == htobeat){
                            mylist.Add(m);
                            htobeat = temph;
                            
                        }
                    }
                    int pick = g.Next(mylist.Count());
                    queenPositions[n] = mylist.ElementAt(pick);

                }
            
            }
            return queenPositions;
            
        }


        //SIMULATED ANNEALING
        public int[] annealing(int numOfIterations, double temperature, double coolingFactor)
        {



            int htobeat = h(queenPositions);


            for (int x = 0; x < numOfIterations && htobeat > 0; x++)
            {
                queenPositions = makeMove(queenPositions, htobeat, temperature);
                htobeat = h(queenPositions);
                temperature = temperature - coolingFactor;
            }

           for (int i = 0; i < bestheuristics.Count(); i++)
            {
                if (h(bestheuristics[i]) == 0)
                {
                    return bestheuristics[i];
                }


            }

            return queenPositions;
        }

        public int[] makeMove(int[] board, int htobeat, double temperature)
        {


            counter = 0;
            

            while (true)
            {

                int nCol = RandomNumber(k);
                int nRow = RandomNumber(k);
                int tmpRow = queenPositions[nCol];
                queenPositions[nCol] = nRow;

                int cost = h(queenPositions);
               
                if (cost < htobeat)
                {
                    //bestheuristics[counter] = (int[])queenPositions.Clone();
                    bestheuristics.Add(queenPositions);
                    counter++;
                    return queenPositions;
                }


                int dE = cost - htobeat;
                double acceptProb = Math.Min(1, Math.Exp(-dE / temperature));
                double rand = random.NextDouble();

                if (rand < acceptProb)
                    return queenPositions;

                queenPositions[nCol] = tmpRow;
            }
            

        }
        
       // K BEAM

        public int[] kbeamsearch(int numOfStates, int numOfIterations)
        {

            int[] tempSort = new int[numOfStates];
            int[][] states = new int[numOfStates][];

            for (int i = 0; i < numOfStates; i++)
                states[i] = randomQueen();//random queen states


            for (int x = 0; x < numOfIterations; x++)
            {

                int[][] newStates = new int[k * numOfStates][];
                for (int i = 0; i < numOfStates; i++)
                {
                    int htobeat = h(states[i]);

                    // if solved
                    if (htobeat == 0)
                    {
                        queenPositions = states[i];
                        return queenPositions;
                    }
                        

                    for (int col = 0; col < k; col++)
                    {
                        newStates[i * k + col] = makeMoveForBeam(states[i], col, htobeat);

                        // if stuck
                        if (newStates[i * k + col] == null)
                            newStates[i * k + col] = randomQueen();
                    }

                }


                List<int> tobesorted = new List<int>(newStates.Count());
                List<int> locations = new List<int>(newStates.Count());
                List<int> finalsort = new List<int>(newStates.Count());

                for (int i = 0; i < newStates.Count(); i++)
                {

                    int temph = h(newStates[i]);
                    tobesorted.Add(temph);

                }
                locations = tobesorted;
                tobesorted.Sort();
                
                for (int i = 0; i < tobesorted.Count(); i++)
                {
                    for (int j = 0; j < tobesorted.Count(); j++)
                    {
                        if (tobesorted.ElementAt(i) == locations.ElementAt(j))
                        {
                            finalsort.Add(j);
                            locations[j] = -1;
                            break;
                        }

                    }

                }




                for (int i = 0; i < states.Count(); i++)
                {



                    states[i] = newStates[finalsort.ElementAt(i)];




                }


                //Array.Sort(newStates,newStates[]);
                //Array.Sort(newStates);
                // Array.Copy(newStates , states , states[0].Length );


            }

            return null;

        }


        public int[] makeMoveForBeam(int[] r, int col, int htobeat)
        {
            int n = r.Length;

            for (int row = 0; row < n; row++)
            {
                // we do not need to evaluate because we already know current cost by costToBeat.
                if (row == r[col])
                    continue;

                int tmpRow = r[col];
                r[col] = row;
                int cost = h(r);
                if (htobeat > cost)
                {
                    r[col] = row;
                    return r;
                }
                r[col] = tmpRow;
            }

            return null;
        }

        public int[][] sortFunc(int[][] tosort, int size) {
            
            int[][] temp = new int[size][];
            List<int> tobesorted = new List<int>();
            List<int> locations = new List<int>();
            List<int> finalsort = new List<int>();

            temp = tosort;

            for (int i = 0; i < tosort.Count(); i++)
            {

                int temph = h(tosort[i]);
                tobesorted.Add(temph);

            }
            locations = tobesorted;
            tobesorted.Sort();

            for (int i = 0; i < tobesorted.Count(); i++)
            {
                for (int j = 0; j < tobesorted.Count(); j++)
                {
                    if (tobesorted.ElementAt(i) == locations.ElementAt(j))
                    {
                        finalsort.Add(j);
                        locations[j] = -1;
                        break;
                    }

                }

            }

            for (int i = 0; i < tosort.Count(); i++)
            {
                tosort[i] = temp[finalsort.ElementAt(i)];
            }

            return tosort;
        }

       
        //GENETİC ALGORTHIM

        
       
        public int[] geneticAlgorithm(int size , double elitism, double crossover , double mutation , int numOfGenerations) {
            int[][] states = new int[size][];
            int[][] temp = new int[size][];
            int[] randomPoss = new int[k];
            int count = 0;
            
           
            int[] chro11;
            int[] chro21;
            int[] chro12;
            int[] chro22;
            

             //random set of Queens
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < k; j++)
                {
                    randomPoss[j] = RandomNumber(k);
                }
                temp[i] = (int[])randomPoss.Clone();
                
                

            }

            states = (int[][])temp.Clone();

            while (count < numOfGenerations) {
                
                int[][] geneticStates = new int[size][];
                int j = 0;
                
                
               
                
              ///////  //-----> SORT PART <------////////////
               

                List<int> tobesorted = new List<int>();
                List<int> locations = new List<int>();
                List<int> finalsort = new List<int>();
                

                for (int i = 0; i < states.Count(); i++)
                {

                    int temph = h(states[i]);
                    tobesorted.Add(temph);

                }
              
                locations = new List<int>(tobesorted);
                tobesorted.Sort();

                for (int i = 0; i < tobesorted.Count(); i++)
                {
                    for (int z = 0; z < locations.Count(); z++)
                    {
                        if (tobesorted.ElementAt(i) == locations.ElementAt(z))
                        {
                            finalsort.Add(z);
                            locations[z] = -1;
                            break;
                        }

                    }

                }

                for (int i = 0; i < states.Count(); i++)
                {
                    states[i] = (int[])temp[finalsort.ElementAt(i)].Clone();
                }
               //////// //-----> SORT PART <------////////////
                
                
                
                //Elitism selection
                if (rand.NextDouble() < elitism ) {
                    geneticStates[0] = states[j];
                    geneticStates[1] = states[j + 1];
                    geneticStates[2] = states[j + 2];
                    geneticStates[3] = states[j + 3];
                    j = 4;              
                }


                int[] currentbest = states[0];
                queenPositions = currentbest;
               
                //end condition
                if (h(currentbest) == 0) {
                    
                    return queenPositions;
                }
                    

                for ( int i=j ; i < size ; ) {
                    int[] chro1 = states[i];
                    int[] chro2 = states[i + 1];
                        if(rand.NextDouble() < crossover){
                             crossoverFunc(ref chro1, ref chro2);
                             chro11 = chro1;
                             chro21 = chro2;
                        }
                        else
                        {
                             chro11=chro1;;
                             chro21=chro2;;   
                        }

                        if(rand.NextDouble() < mutation){
                            mutationFunc(ref chro11, ref chro21);
                            chro12 = chro11;
                            chro22 = chro21;
                        
                        }
                        else{
                            chro12= chro11;
                           chro22= chro21;
                        }

                        geneticStates[i] = chro12;
                        geneticStates[i+1] = chro22;
                   
                    i++;
                    i++;
                }

                

                states =(int[][])geneticStates.Clone();
                temp = (int[][])geneticStates.Clone();
                count++;
            }


            return states[0];
        
        }

        public void crossoverFunc(ref int[] c1, ref int[] c2) {
            Random rand = new Random();
            int crosspoint = rand.Next(k);
            int crosspoint2 = rand.Next(k);
            int cmin = Math.Min(crosspoint, crosspoint2);
            int cmax = Math.Max(crosspoint2, crosspoint);
            int[] X = new int[k];
            int[] Y = new int[k];

            for (int i = 0; i < k; i++) {
                if (i < cmin)
                    X[i] = c1[i];
                else if (i > cmin && i < cmax)
                    X[i] = c2[i];
                else if (i > cmax)
                    X[i] = c1[i];
            }
            
            for (int i = 0; i < k; i++)
            {
                if (i < cmin)
                    Y[i] = c2[i];
                else if (i > cmin && i < cmax)
                    Y[i] = c1[i];
                else if (i > cmax)
                    Y[i] = c2[i];
            }

            c1 = X;
            c2 = Y;
        }

        public void mutationFunc(ref int[] c1, ref int[] c2)
        {
            Random rand = new Random();
            int mutationpointX = rand.Next(k);
            int mutationpointY = rand.Next(k);

            c1[mutationpointX] = rand.Next(k);
            c2[mutationpointY] = rand.Next(k);


        }

     

    }
}
