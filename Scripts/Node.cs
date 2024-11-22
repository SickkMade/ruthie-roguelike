using UnityEngine;

public class Node 
{
    public RectInt rect;
    public RectInt? room = null;
    public Node left, right;

    public Node(RectInt rect){
        this.rect=rect;
    }
}
