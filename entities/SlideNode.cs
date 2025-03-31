using Godot;
using System.Collections.Generic;
using System;
using System.Linq;

public partial class SlideNode : CollisionPolygon2D
{    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Polygon2D polygon = GetNode<Polygon2D>("Polygon2D");  
        polygon.Polygon = new Vector2[Polygon.Length + 1];  

        Vector2 appendValue = Polygon[0];
        Vector2[] linePoints = new Vector2[Polygon.Length + 1];

        Polygon.CopyTo(linePoints, 0);
        

        linePoints[linePoints.Length - 1] = appendValue;
        

        polygon.Polygon = linePoints;

        foreach( Vector2 vec in linePoints) {
            GD.Print(vec);
        }
	}
}
