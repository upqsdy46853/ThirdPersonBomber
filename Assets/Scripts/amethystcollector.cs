using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class amethystcollector : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnCountChanged))]
    int collect_count { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        collect_count = 0;
    }
    public int getCount()
    {
        return collect_count;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
            return;

        Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(5.0f, 1.5f, 5.0f));
        int amethyst_count = 0;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (!hitColliders[i].gameObject.TryGetComponent<Amethyst>(out var amethyst))
                continue;
            amethyst_count += 1;
        }
        collect_count = amethyst_count;

    }

    public void on_collect_remote(int count)
    {
        collect_count = count;
        Debug.Log("OnContChanged" + collect_count);
    }
    static void OnCountChanged(Changed<amethystcollector> changed)
    {
        int count = changed.Behaviour.collect_count;
        changed.Behaviour.on_collect_remote(count);
    }
}
