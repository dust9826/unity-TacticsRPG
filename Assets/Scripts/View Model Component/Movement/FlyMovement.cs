using System.Collections;
using UnityEngine;

public class FlyMovement : Movement
{
    public override IEnumerator Traverse(Tile tile)
    {
        float xMathfPow = Mathf.Pow(tile.pos.x - unit.tile.pos.x, 2);
        float yMathfPow = Mathf.Pow(tile.pos.y - unit.tile.pos.y, 2);

        float dist = Mathf.Sqrt(xMathfPow + yMathfPow);
        unit.Place(tile);

        float y = Tile.stepHeight * 10;
        float duration = (y - jumper.position.y) * 0.5f;
        Vector3 moveToPosition = new Vector3(0, y, 0);

        Tweener tweener = jumper.MoveToLocal(moveToPosition, duration, EasingEquations.EaseInOutQuad);

        while (tweener != null)
            yield return null;

        Directions dir;
        Vector3 toTile = (tile.center - transform.position);

        if (Mathf.Abs(toTile.x) > Mathf.Abs(toTile.z))
            dir = toTile.x > 0 ? Directions.East : Directions.West;
        else
            dir = toTile.z > 0 ? Directions.North : Directions.South;
        yield return StartCoroutine(Turn(dir));

        duration = dist * 0.5f;
        tweener = transform.MoveTo(tile.center, duration, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;

        duration = (y - tile.center.y) * 0.5f;
        tweener = jumper.MoveToLocal(Vector3.zero, 0.5f, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;
    }
}
