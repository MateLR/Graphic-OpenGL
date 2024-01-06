using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq.Expressions;
using static Task2.Drawer;

namespace Task2
{
    public partial class Form1 : Form
    {
        private float rt = 0;
        private SharpGL.SceneGraph.Assets.Texture texture;
        private IntPtr earth;
        public Form1()
        {
            InitializeComponent();
        }

        private void openGLControl1_Load(object sender, EventArgs e)
        {
        }

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            
        }

        private float s = 1;
        private float delta = 0.01f;
        private int x = 1;
        private int state = 0;
        private Boolean rt_x = false;
        private Boolean rt_y = true;
        private Boolean rt_z = false;
        private int n = 3;


        private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            OpenGL gl = this.openGLControl1.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.Translate(0.0f, 0.0f, -5.0f);
            gl.Rotate(delta, rt_x ? 1f : 0f , rt_y ? 1f : 0f, rt_z ? 1f : 0f);
            delta -= 0.2f * trackBar1.Value;
            draw(gl, earth, trackBar1.Value);
            gl.End();
            gl.Flush();
        }

        private void refresh()
        {
            delta = 0.01f;
            s = 1;
            n = 3;
            x = 1;
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            state += 1;
            if (state > 4)
                state = 0;
            refresh();
        }

        private void openGLControl1_Load_1(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            rt_x = !rt_x;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            rt_y =!rt_y;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            rt_z = !rt_z;
        }

        private void openGLControl1_OpenGLInitialized_1(object sender, EventArgs e)
        {
            var gl = this.openGLControl1.OpenGL;
            texture = new SharpGL.SceneGraph.Assets.Texture();
            texture.Create(gl, "earth.bmp");
            earth = gl.NewQuadric();
            gl.QuadricNormals(earth, OpenGL.GL_SMOOTH);
            gl.QuadricTexture(earth, (int)(OpenGL.GL_TRUE));
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            
        }
    }
}
