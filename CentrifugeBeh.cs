using Il2Cpp;
using UnityEngine;

//namespace IndustrialHorizons;

public class CentrifugeBeh : MonoBehaviour
{
  public CentrifugeBeh(System.IntPtr ptr)
    : base(ptr)
  {
  }

  public Substance refinedSubstance { get; set; }
  //public double meltTemperature { get; set; } = 600;

  private void OnInitialize()
  {
    this.cubeBase = base.GetComponent<CubeBase>();
    this.cubeBase.enabled = true;
  }

  private void Start()
  {
  }

  private void Update()
  {
    bool flag = !this.updated;
    if (flag)
    {
      bool flag2 = this.cubeBase.heat.GetCelsiusTemperature() > 100;
      if (flag2)
      {
        this.cubeBase.ChangeSubstance(this.refinedSubstance);
        this.updated = true;
      }
    }
  }

  // Token: 0x04000009 RID: 9
  private CubeBase cubeBase;

  // Token: 0x0400000B RID: 11
  private bool updated = false;
}