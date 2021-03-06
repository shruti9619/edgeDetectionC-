using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using System.IO;
using System.Drawing.Imaging;

namespace camera1
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection capturedevices;
        private VideoCaptureDevice chosendevice;
        private Bitmap image;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            capturedevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo VideoCaptureDevice in capturedevices)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);

                comboBox1.SelectedIndex = 0;
            }

          //  Bitmap b = new Bitmap("C:\\Users\\shrutii\\Documents\\Bluetooth Folder\\cjsk.jpg");
          // //  create grayscale filter (BT709)
          //  Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
          //  // apply the filter
          //  Bitmap g = filter.Apply(b);

          // // edge
          ////   create filter
          //  HomogenityEdgeDetector filters = new HomogenityEdgeDetector();
          //  // apply the filter
          //  filters.ApplyInPlace(g);
          //  //Bitmap c = b + g;
          //  pictureBox1.Image = g;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chosendevice = new VideoCaptureDevice(capturedevices[comboBox1.SelectedIndex].MonikerString);
            chosendevice.NewFrame += new NewFrameEventHandler(chosendevice_newframe);
            chosendevice.Start();
        }

        void chosendevice_newframe(object sender,NewFrameEventArgs eventargs)
        {
           // throw new NotImplementedException();
            image = (Bitmap)eventargs.Frame.Clone();

            // create grayscale filter (BT709)
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            // apply the filter
            Bitmap grayImage = filter.Apply(image);

            //edge
            // create filter
            DifferenceEdgeDetector filters = new DifferenceEdgeDetector();
            // apply the filter
            filters.ApplyInPlace(grayImage);

            pictureBox1.Image = grayImage;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(chosendevice.IsRunning)
            { chosendevice.Stop(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //saveFileDialog1.ShowDialog();

            pictureBox1.Image.Save("a.bmp");
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
           
        }
        
    }

}
