using System;
using System.Drawing;

/**
 * <p>Title: FTable</p>
 *
 * <p>Description: </p>
 *
 * <p>Copyright: Copyright (c) 2006</p>
 *
 * <p>Company: </p>
 *
 * @author Kurt Wolfe
 * @version 1.0
 */
public class CircleChannel //: JPanel
{
  private double _diameter;
  protected Color _color;
  protected double _diam;

  public CircleChannel(double diameter)
  {
    _diameter = diameter;
    _color = Color.Red;
    activateToolTipText();
  }

  public CircleChannel(double diameter, Color color)
  {
    _diameter = diameter;
    _color = color;
    activateToolTipText();
  }
  private void activateToolTipText(){
	  this.setToolTipText("Circular Channel Diagram"); 
  }
  
  public void setDiameter(double diameter)
  {
    _diameter = diameter;
  }

  //public void paintComponent (Graphics g)
  //{
  //  super.paintComponent (g);

  //  g.setColor (_color);

  //  int width = this.getWidth();
  //  int height = this.getHeight();


  //  if (width >= height)
  //  {
  //    if (height > 20)
  //     _diam = height - 10;
  //    else
  //      _diam = height - 10;

  //  }

  //  if (height > width)
  //  {
  //    if (width > 20)
  //      _diam = width - 10;
  //    else
  //      _diam = width;

  //  }

  //  int xPt, yPt;
  //  xPt = (width - (int)_diam)/2;
  //  yPt = (height - (int)_diam)/2;
  //  g.drawOval(xPt, yPt, (int)_diam,(int)_diam);

  //  g.setColor (Color.black);
  //  g.drawLine((width/2)-(int)_diam/2,height/2,(width/2)+((int)(_diam/2)),height/2);
  
  //  string diam = double.toString(_diameter);
  //  g.drawString(diam,width/2-5, height/2-5);

  //}




 // public static void main(string[] args)
  //{
  //  CircleChannel circlechannel = new CircleChannel(1.0);
  //}
}
