using UnityEngine;

[RequireComponent(typeof(HarpoonComponent))]
public class HarpoonVisualComponent : MonoBehaviour
{
    public LineRenderer HarpoonLine;
    public GameObject HarpoonHead;

    HarpoonComponent harpoon;

    void Start()
    {
        harpoon = GetComponent<HarpoonComponent>();
        // line bottom vertex position 
        HarpoonLine.SetPosition(0, harpoon.transform.position);
        HarpoonLine.startWidth = harpoon.HarpoonHeadSize.x;
        HarpoonLine.endWidth = harpoon.HarpoonHeadSize.x;
    }

    void Update()
    {
        var headPosition = harpoon.transform.position;
        headPosition.y += harpoon.HarpoonLength - harpoon.HarpoonHeadSize.y;

        // line top vertex position 
        HarpoonLine.SetPosition(1, headPosition);

        HarpoonHead.transform.position = headPosition + new Vector3(0, harpoon.HarpoonHeadSize.y / 2, 0);
    }
}
