﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : Movement
{
    protected override bool ExpandSearch(Tile from, Tile to)
    {
        if (Mathf.Abs(from.height - to.height) > jumpHeight)
            return false;

        if (to.content != null)
            return false;

        return base.ExpandSearch(from, to);
    }

    public override IEnumerator Traverse(Tile tile)
    {
        unit.Place(tile);

        List<Tile> targets = new List<Tile>();
        while(tile != null)
        {
            targets.Insert(0, tile);
            tile = tile.prev;
        }

        for(int i=1;i<targets.Count;++i)
        {
            Tile from = targets[i - 1];
            Tile to = targets[i];

            Directions dir = from.GetDirections(to);

            if(unit.dir != dir)
            {
                yield return StartCoroutine(Turn(dir));
            }

            if(from.height == to.height)
            {
                yield return StartCoroutine(Walk(to));
            }
            else
            {
                yield return StartCoroutine(Jump(to));
            }
        }
        yield return null;
    }

    IEnumerator Walk(Tile target)
    {
        Tweener tweener = transform.MoveTo(target.center, 0.5f, EasingEquations.Linear);
        while (tweener != null)
            yield return null;
    }

    IEnumerator Jump(Tile to)
    {
        Tweener tweener = transform.MoveTo(to.center, 0.5f, EasingEquations.Linear);

        Vector3 stepHeightVec = new Vector3(0, Tile.stepHeight * 2f, 0);
        float destinationTweener = tweener.easingControl.duration / 2f;

        Tweener t2
            = jumper.MoveToLocal(stepHeightVec, destinationTweener, EasingEquations.EaseOutQuad);
        t2.easingControl.loopCount = 1;
        t2.easingControl.loopType = EasingControl.LoopType.PingPong;

        while (tweener != null)
            yield return null;
    }
}
