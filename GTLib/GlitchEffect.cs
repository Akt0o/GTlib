using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GTLib
{
    /// <summary>
    /// Main class of the GTLib lib. Used to create a glitch effect on a string inside a canvas.
    /// </summary>
    public class GlitchEffect
    {
        //height of the text (graphicaly)
        private int height;
        //width of the text (graphicaly)
        private int width;
        //Color of the text (string)
        private System.Drawing.Color color;
        //Matrix of boolean, true = pixel with color, false = empty pixel.
        private bool[,] pixelMatrix;
        //list of frames generated 
        private List<ImageBrush> states;
        //Index of the current frame
        private int currentFrame;
        //Fontsize of the text
        private int fontSize;

        /// <summary>
        /// Constructor of the GlitchEffect class
        /// </summary>
        public GlitchEffect()
        {
            pixelMatrix = null;
            states = new List<ImageBrush>();
            color = System.Drawing.Color.Black;
            height = 0;
            width = 0;
            currentFrame = 0;
            fontSize = 16;
        }

        /// <summary>
        /// Function used to set the text that will be altered by the GlitchEffect Class.
        /// </summary>
        /// <param name="text">the actual text (string)</param>
        /// <param name="fontName">the font of the text (string)</param>
        /// <param name="fontSize">the font size (int)</param>
        /// <param name="hexColor">the color of the text in hex (string)</param>
        public void setText(string text,string fontName, int fontSize, string hexColor)
        {
            color = ColorTranslator.FromHtml(hexColor);
            this.fontSize = fontSize;
            Font font = new Font(fontName, fontSize);
            SizeF textSize = Graphics.FromImage(new Bitmap(1, 1)).MeasureString(text, font);

            width = (int)textSize.Width;
            height = (int)textSize.Height;

            pixelMatrix = new bool[width, height];
            //We get the values of the matrix/2DArray of boolean here.
            using (Bitmap bmp = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                    g.DrawString(text, font, System.Drawing.Brushes.Black, PointF.Empty);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        System.Drawing.Color pixelColor = bmp.GetPixel(x, y);
                        bool isPixelSet = pixelColor.A != 0;
                        pixelMatrix[x, y] = isPixelSet;
                    }
                }
            }

        }

        /// <summary>
        /// Function used to generate the frames inside the GlitchEffect object (must have used setText function before)
        /// </summary>
        /// <param name="frames">Number of frames generated (int) </param>
        /// <param name="intensity">Intensity of the glitch effect (int)</param>
        /// <param name="decal">quantity of offset on the pixels affected by the glitch effect (int)</param>
        public void GenerateFrames(int frames, int intensity, int decal) 
        {
            if(states is not null) states.Clear();

            Random rand = new Random();

            //We change the values of pixelMatrixCopy to apply a glitched pixalted effect.
            for (int i = 0; i < frames; i++)
            {
                bool[,] pixelMatrixCopy = (bool[,])pixelMatrix.Clone();

                List<int> temp = new List<int>();
                int newVal;

                for (int num = 0; num < intensity; num++)
                {
                    if (rand.Next(0, 3) == 0)
                    {
                        newVal = rand.Next(Convert.ToInt32(pixelMatrixCopy.GetLength(1) * 0.8));
                        temp.Add(newVal);
                        for (int val = newVal; val < (newVal+fontSize/16); val++)
                        {
                            if(val<pixelMatrixCopy.GetLength(0)) temp.Add(val);
                        }
                    }
                }

                for (int row = 0; row < pixelMatrixCopy.GetLength(0); row++)
                {
                    for (int col = decal; col < pixelMatrixCopy.GetLength(1); col++) if (temp.Contains(col)) pixelMatrixCopy[row, col] = pixelMatrixCopy[row, col - decal];
                    for (int col = 0; col < decal; col++) if (temp.Contains(col)) pixelMatrixCopy[pixelMatrix.GetLength(1) - 1 + col, col] = true;
                }

                //We get the bitmap from the altered matrix

                Bitmap bmp2 = new Bitmap(width, height);

                // Loop through every pixel of the new bitmap to apply the values of the matrix
                for (int y = 0; y < bmp2.Height; y++)
                    for (int x = 0; x < bmp2.Width; x++)
                        if (pixelMatrixCopy[x, y]) bmp2.SetPixel(x, y, color);


                BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                bmp2.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

                // Create a new ImageBrush object and set its ImageSource property
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = bitmapSource;

                states.Add(imageBrush);
            }
        }

        /// <summary>
        /// Function used to get the next frame generated by the function GenerateFrames.
        /// Simply put the returned value as the background of your canvas.
        /// </summary>
        /// <returns>The next frame generated by the GenerateFrames function.(ImageBrush type).</returns>
        public ImageBrush GetNextFrame()
        {
            ImageBrush temp = states[currentFrame];
            currentFrame++;
            if (currentFrame >= states.Count()) currentFrame = 0;
            return temp;
        }
    }
}
