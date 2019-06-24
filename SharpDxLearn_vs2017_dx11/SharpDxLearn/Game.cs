using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Windows;
using System.Drawing;

public class Game : IDisposable
{
    RenderForm m_renderForm;

    int m_width = 1280;
    int m_height = 720;

    public Game()
    {
        m_renderForm = new RenderForm("SharpDxLearn");
        m_renderForm.ClientSize = new Size(m_width, m_height);
        m_renderForm.AllowUserResizing = false;
    }

    public void Run()
    {
        RenderLoop.Run(m_renderForm, RenderCallback);
    }

    void RenderCallback()
    {

    }

    public void Dispose()
    {
        m_renderForm.Dispose();
    }
}