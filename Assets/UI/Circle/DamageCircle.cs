﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using UnityEngine;

public class DamageCircle : MonoBehaviour {

    private static DamageCircle instance;

    [SerializeField] private Transform targetCircleTransform;

    private Transform circleTransform;
    private Transform topTransform;
    private Transform bottomTransform;
    private Transform leftTransform;
    private Transform rightTransform;

    private float circleShrinkSpeed;
    private float shrinkTimer;

    private Vector3 circleSize;
    private Vector3 circlePosition;

    private Vector3 targetCircleSize;
    private Vector3 targetCirclePosition;
    bool last_circle = false;


    private void Awake() {
        instance = this;

        circleShrinkSpeed = 5f;

        circleTransform = transform.Find("circle");
        topTransform = transform.Find("top");
        bottomTransform = transform.Find("bottom");
        leftTransform = transform.Find("left");
        rightTransform = transform.Find("right");

        SetCircleSize(new Vector3(0, 0), new Vector3(50, 50));

        SetTargetCircle(new Vector3(0, 0), new Vector3(45, 45), 5f);
    }

    private void Update() {
        shrinkTimer -= Time.deltaTime;

        if (shrinkTimer < 0 && last_circle == false) {
            Vector3 sizeChangeVector = (targetCircleSize - circleSize).normalized;
            Vector3 newCircleSize = circleSize + sizeChangeVector * Time.deltaTime * circleShrinkSpeed;

            Vector3 circleMoveDir = (targetCirclePosition - circlePosition).normalized;
            Vector3 newCirclePosition = circlePosition + circleMoveDir * Time.deltaTime * circleShrinkSpeed;
            if(newCircleSize.x <= 10f)
            {
                newCircleSize = new Vector3(10f, 10f);
                newCirclePosition = targetCirclePosition;
                last_circle = true;
            }
            SetCircleSize(newCirclePosition, newCircleSize);

            float distanceTestAmount = .1f;
            if (Vector3.Distance(newCircleSize, targetCircleSize) < distanceTestAmount && Vector3.Distance(newCirclePosition, targetCirclePosition) < distanceTestAmount) {
                GenerateTargetCircle();
            }
        }
    }

    private void GenerateTargetCircle() {
        float shrinkSizeAmount = Random.Range(1f, 5f);
        Vector3 generatedTargetCircleSize = circleSize - new Vector3(shrinkSizeAmount, shrinkSizeAmount) * 2f;

        // Set a minimum size
        if (generatedTargetCircleSize.x < 10f)
        {
            generatedTargetCircleSize = Vector3.one * 10f;
            shrinkSizeAmount = (circleSize.x - 10f) / 2f;
        }

        Vector3 generatedTargetCirclePosition = circlePosition + 
            new Vector3(Random.Range(-shrinkSizeAmount, shrinkSizeAmount), Random.Range(-shrinkSizeAmount, shrinkSizeAmount));

        float shrinkTime = Random.Range(1f, 5f);

        SetTargetCircle(generatedTargetCirclePosition, generatedTargetCircleSize, shrinkTime);
    }

    private void SetCircleSize(Vector3 position, Vector3 size) {
        circlePosition = position;
        circleSize = size;

        transform.position = position;

        circleTransform.localScale = size;

        topTransform.localScale = new Vector3(500, 500);
        topTransform.localPosition = new Vector3(0, topTransform.localScale.y * .5f + size.y * .5f);
        
        bottomTransform.localScale = new Vector3(500, 500);
        bottomTransform.localPosition = new Vector3(0, -topTransform.localScale.y * .5f - size.y * .5f);

        leftTransform.localScale = new Vector3(500, size.y);
        leftTransform.localPosition = new Vector3(-leftTransform.localScale.x * .5f - size.x * .5f, 0f);

        rightTransform.localScale = new Vector3(500, size.y);
        rightTransform.localPosition = new Vector3(+leftTransform.localScale.x * .5f + size.x * .5f, 0f);
    }

    private void SetTargetCircle(Vector3 position, Vector3 size, float shrinkTimer) {
        this.shrinkTimer = shrinkTimer;

        targetCircleTransform.position = position;
        targetCircleTransform.localScale = size;
        
        targetCirclePosition = position;
        targetCircleSize = size;
    }

    private bool IsOutsideCircle(Vector3 position) {
        return Vector3.Distance(position, circlePosition) > circleSize.x * .5f;
    }

    public static bool IsOutsideCircle_Static(Vector3 position) {
        return instance.IsOutsideCircle(position);
    }



}