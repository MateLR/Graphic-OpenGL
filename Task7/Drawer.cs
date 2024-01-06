using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SharpGL;
using static Task2.Drawer;

namespace Task2
{
    public static class Drawer
    {
        public static int timer;
        static float[] earth_mat_ambient   = {0.0f, 0.0f, 1.0f, 1.0f};
        static float[] earth_mat_diffuse   = {0.0f, 0.0f, 0.5f, 1.0f};
        static float[] earth_mat_specular = {1.0f, 0.5f, 0.0f, 1.5f}; 
        static float[] earth_mat_emission = {0f, 0.0f, 0.0f, 1.0f}; 
        static float earth_mat_shininess   = 128.0f;
        static float[] sun_light_position = { 0f, 0f, 0f, 1f };//Light source position, positional point light source
        static float[] sun_light_ambient = { 1f, 0f, 0f, 1f };//Ambient light
        static float[] sun_light_diffuse = { 1f, 1f, 1f, 1f };//Diffuse light
        static float[] sun_light_speular = { 1f, 1f, 1f, 1f };//Mirror light
        private static IntPtr _earth;
        public static void draw(OpenGL gl,IntPtr earthT, int trackBar1Value)
        {
            _earth = earthT;
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Color(1f,0.5f,0f);
            draw_sphere(gl, 0.5f, sun_light_position[0], sun_light_position[1], sun_light_position[2],gl.NewQuadric(),0);//Draw the red sun;
            gl.Enable(OpenGL.GL_LIGHTING);
            timer += 1*trackBar1Value;
            //setLight(gl);
            setLight(gl);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT, earth_mat_ambient);
            //gl.Material(OpenGL.GL_FRONT, OpenGL.GL_DIFFUSE, earth_mat_diffuse);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SPECULAR, earth_mat_specular);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_EMISSION, earth_mat_emission);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SHININESS, earth_mat_shininess);
            gl.Rotate(-timer,1f,0f,1f);
            draw_sphere(gl, 0.3f, 1f, 0f, 1f, _earth,1);
            gl.Rotate(-timer,-1f,0f,-1f);
            draw_ufo(gl, 1.1f, 0.52f, 1f);
            gl.Disable(OpenGL.GL_LIGHTING);
            draw_galaxy(gl);
            draw_sun(gl);
            draw_ufo_trace(gl, 1.1f, 0.35f, 1f);
        }

        private static void setLight(OpenGL gl)
        {
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, sun_light_position);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, sun_light_ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, sun_light_diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, sun_light_speular);

            gl.Enable(OpenGL.GL_LIGHT0);//Turn on the light source 0
            gl.Enable(OpenGL.GL_LIGHTING);//Turn on the light source function
        }

        private static void draw_galaxy(OpenGL gl)
        {
            gl.PointSize(1f);
            gl.Begin(OpenGL.GL_POINTS);
            float[] clr1 = { 1.0f, 0.1f, 1.0f };
            float[] clr2 = { 0.0f, 0.1f, 1.0f };
            if (timer % 8 > 4)
                (clr2, clr1) = (clr1, clr2);
            var isColor = true;
            for (var u = -Math.PI; u <= Math.PI; u += 0.1)
            { 
                for (var v = -Math.PI / 2; v <= Math.PI / 2; v += 0.1)
                {
                    var a = (Math.Sin(v) + 1.0) / 2.0;
                    gl.Color(isColor ? clr1 : clr2);
                    gl.Vertex(Math.Cos(v) * Math.Cos(u) * 50, Math.Sin(v) * 50, Math.Cos(v) * Math.Sin(u) * 50);
                    isColor = !isColor;
                }
                isColor = !isColor;
            }
            gl.End();
        }

        private static void draw_sun(OpenGL gl)
        {
            gl.PointSize(2f);
            gl.Begin(OpenGL.GL_POINTS);
            float[] clr1 = {1f, 0.2f, 0.2f};
            var r = 0.59;
            var v = timer % 100 / 15.9 - Math.PI / 2;
            for (var u = -Math.PI; u <= Math.PI; u += 0.1)
            {
                gl.Color(clr1);
                gl.Vertex(Math.Cos(v) * Math.Cos(u) * r, Math.Sin(v)* Math.Cos(u) * r, Math.Cos(v) * Math.Sin(u) * r);
                gl.Vertex(Math.Cos(u) * Math.Cos(v) * r, Math.Sin(u)* Math.Cos(v) * r, Math.Cos(u) * Math.Sin(v) * r);
                v = - v;
                gl.Vertex(Math.Cos(v) * Math.Cos(u) * r, Math.Sin(v)* Math.Cos(u) * r, Math.Cos(v) * Math.Sin(u) * r);
                gl.Vertex(Math.Cos(u) * Math.Cos(v) * r, Math.Sin(u)* Math.Cos(v) * r, Math.Cos(u) * Math.Sin(v) * r);
            }
            gl.End();
        }

        private static void draw_sphere(OpenGL gl,double radius, float xPos, float yPos, float zPos, IntPtr sphere, int x)
        {
            if (x > 0)
                gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.PushMatrix();
            gl.Translate(xPos, yPos, zPos);
            gl.QuadricDrawStyle(sphere, OpenGL.GL_POINT);
            gl.QuadricNormals(sphere, OpenGL.GLU_FLAT);   //GLU_NONE,GLU_FLAT,GLU_SMOOTH
            gl.QuadricOrientation(sphere, (int)OpenGL.GLU_OUTSIDE);  //GLU_OUTSIDE,GLU_INSIDE
            gl.QuadricTexture(sphere, x);  //GL_TRUE,GLU_FALSE
            gl.Sphere(sphere, radius, 40, 40);
            //gl.DeleteQuadric(earth);
            gl.PopMatrix();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        private static void draw_ufo(OpenGL gl, float xPos, float yPos, float zPos)
        {
            gl.Begin(OpenGL.GL_QUADS);
            var scale1 = 0.15f;
            var scale2 = 0.06f;
            var last = 0.0;
            for (var u = 0 + Math.PI/5; u <= 2 * Math.PI; u += Math.PI/5)
            {
                gl.Color(0f, 1f, 1f);
                gl.Vertex(Math.Cos(u) * scale1 + xPos, -1f * scale1 + yPos, Math.Sin(u)* scale1 + zPos);
                gl.Color(0f, 1f, 0f);
                gl.Vertex(Math.Cos(u) * scale2+ xPos, 1f * scale2 + yPos - 0.13f, Math.Sin(u)/2 * scale2 + zPos);
                gl.Vertex(Math.Cos(last) * scale2+ xPos, 1f * scale2 + yPos - 0.13f, Math.Sin(last)/2 * scale2+ zPos);
                gl.Color(0f, 1f, 1f);
                gl.Vertex(Math.Cos(last) * scale1+ xPos, -1f * scale1 + yPos, Math.Sin(last)* scale1+ zPos);
                gl.Vertex(Math.Cos(u) * scale1+ xPos, -1f * scale1 + yPos, Math.Sin(u)* scale1+ zPos);
                last = u;
            }
            gl.End();
            gl.PushMatrix();
            gl.Translate(xPos, yPos-0.04f, zPos);
            var sphere = gl.NewQuadric();
            gl.QuadricDrawStyle(sphere, OpenGL.GL_POINT);
            gl.QuadricNormals(sphere, OpenGL.GLU_FLAT);   //GLU_NONE,GLU_FLAT,GLU_SMOOTH
            gl.QuadricOrientation(sphere, (int)OpenGL.GLU_OUTSIDE);  //GLU_OUTSIDE,GLU_INSIDE
            gl.QuadricTexture(sphere, 0);  //GL_TRUE,GLU_FALSE
            gl.Sphere(sphere, scale2, 40, 40);
            //gl.DeleteQuadric(earth);
            gl.PopMatrix();
            
        }
        private static void draw_ufo_trace(OpenGL gl,float xPos, float yPos, float zPos)
        {
            gl.LineWidth(0.1f);
            gl.Begin(OpenGL.GL_LINE_LOOP);
            var scale1 = 0.1f;
            var scale2 = 0.03f;
            var last = 0.0;
            for (var u = 0.1; u <= 2 * Math.PI; u += 0.5)
            {
                gl.Color(0f, 1f, 1f);
                gl.Vertex(Math.Cos(u) * scale1 + xPos, -1f * scale1*1.5 + yPos, Math.Sin(u)* scale1 + zPos);
                gl.Color(0f, 1f, 0f);
                gl.Vertex(Math.Cos(u) * scale2+ xPos, 1f * scale2 + yPos, Math.Sin(u)/2 * scale2 + zPos);
                gl.Vertex(Math.Cos(last) * scale2+ xPos, 1f * scale2 + yPos, Math.Sin(last)/2 * scale2+ zPos);
                gl.Color(0f, 1f, 1f);
                gl.Vertex(Math.Cos(last) * scale1+ xPos, -1f * scale1*1.5 + yPos, Math.Sin(last)* scale1+ zPos);
                gl.Vertex(Math.Cos(u) * scale1+ xPos, -1f * scale1*1.5 + yPos, Math.Sin(u)* scale1+ zPos);
                last = u;
            }
            gl.End();
        }
    }
}