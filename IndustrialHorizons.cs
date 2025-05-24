using MelonLoader;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem;
using System;
using PMAPI;
using UnityEngine;
using PMAPI.CustomSubstances;
using PMAPI.OreGen;
using UnityEngine.SceneManagement;
using System.Text.Json;

namespace IndustrialHorizons
{
    public class IndustrialHorizons : MelonMod
    {
        // Mod data class
        public class OurData
        {
            public int Test { get; set; }

            public OurData(int test)
            {
                Test = test;
            }
        }

        Substance chalcocite;
        Substance copper;
        Substance ilmenite;
        Substance titaniumoxide;
        Substance titanium;
        Substance chromite;
        Substance chromium;
        Substance bauxite;
        Substance aluminum;
        Substance coal;
        Substance steel;
        Substance stainlesssteel;
        public override void OnInitializeMelon()
        {
            // Init PMAPI
            PMAPIModRegistry.InitPMAPI(this);

            // Register in our behaviour in IL2CPP so the game knows about it
            ClassInjector.RegisterTypeInIl2Cpp<OreBeh>(new RegisterTypeOptions
            {
                Interfaces = new System.Type[] { typeof(ICubeBehavior)}
            });

            // Registering modded substances
            this.RegisterChalcocite();
            this.RegisterCopper();
            this.RegisterIlmenite();
            this.RegisterTitaniumoxide();
            this.RegisterTitanium();
            this.RegisterChromite();
            this.RegisterChromium();
            this.RegisterBauxite();
            this.RegisterAluminum();
            this.RegisterCoal();
            this.RegisterSteel();
            this.RegisterStainlessSteel();
            CubeMerge.compoundablePairs.Add(new Il2CppSystem.ValueTuple<Substance, Substance>(this.chromite, Substance.Niter), new Il2CppSystem.ValueTuple<float, Substance, float>(1f, this.chromium, 0.6f));
            CubeMerge.compoundablePairs.Add(new Il2CppSystem.ValueTuple<Substance, Substance>(this.coal, Substance.Iron), new Il2CppSystem.ValueTuple<float, Substance, float>(0.5f, this.steel, 1.2f));
            CubeMerge.compoundablePairs.Add(new Il2CppSystem.ValueTuple<Substance, Substance>(this.chromium, this.steel), new Il2CppSystem.ValueTuple<float, Substance, float>(0.1f, this.stainlesssteel, 1.1f));
            CubeMerge.compoundablePairs.Add(new Il2CppSystem.ValueTuple<Substance, Substance>(this.ilmenite, Substance.Sulfur), new Il2CppSystem.ValueTuple<float, Substance, float>(0.8f, this.titaniumoxide, 0.7f));

            // Registering our substances in ore generation
            CustomOreManager.RegisterCustomOre(this.chalcocite, new CustomOreManager.CustomOreParams
            {
                chance = 0.3f,
                substanceOverride = Substance.Sulfur,
                maxSize = 0.4f,
                minSize = 0.2f,
                alpha = 1f
            });
            CustomOreManager.RegisterCustomOre(this.ilmenite, new CustomOreManager.CustomOreParams
			{
                chance = 0.06f,
				substanceOverride = Substance.TungstenOre,
				maxSize = 0.7f,
				minSize = 0.2f,
				alpha = 1f
			});
            CustomOreManager.RegisterCustomOre(this.chromite, new CustomOreManager.CustomOreParams
			{
				chance = 0.04f,
				substanceOverride = Substance.Niter,
				maxSize = 0.5f,
				minSize = 0.1f,
				alpha = 1f
			});
            CustomOreManager.RegisterCustomOre(this.bauxite, new CustomOreManager.CustomOreParams
			{
				chance = 0.1f,
				substanceOverride = Substance.Clay,
				maxSize = 0.8f,
				minSize = 0.1f,
				alpha = 1f
			});
            CustomOreManager.RegisterCustomOre(this.coal, new CustomOreManager.CustomOreParams
            {
                chance = 0.05f,
                substanceOverride = Substance.Stone,
                maxSize = 0.1f,
                minSize = 1.9f,
                alpha = 1f
            });
        }

        // Gets called just when the world was loaded
        public void OnWorldWasLoaded()
        {
            // Outputting mod data. The question mark means that nothing will be outputted if mod data doesn't exist (data == null)
            MelonLogger.Msg("Mod data is {0}", ExtDataManager.GetData<OurData>()?.Test);
        }

        void RegisterChalcocite()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Stone"))
            {
                name = "Chalcocite (Cu2S)",
                color = new Color(0.35f, 0.71f, 0.51f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.Pyrite).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // overwriteDeez
            param.displayNameKey = "SUB_CALCOCITE";
            param.material = cmat.name;
            param.density = 5.65f;
            param.strength = 90f;
            param.stiffness = 30f;
            param.hardness = 2.5f;
            var description = "Melt deez at 600 degree to make copper";

            // Registering our substance as custom substance
            this.chalcocite = CustomSubstanceManager.RegisterSubstance("chalcocite", param, new CustomSubstanceParams
            {
                enName = "Chalcocite (Cu2S)",
                jpName = "Chalcocite (Cu2S)",
                behInit = delegate (CubeBase cb)
                {
                    // Adding test behavior
                    OreBeh oreBeh = cb.gameObject.AddComponent<OreBeh>();
                    oreBeh.meltTemperature = 600;
                    oreBeh.refinedSubstance = this.copper;
                    return oreBeh;
                }
            });
            
        }

        void RegisterCopper()
        {
            // Getting material
            Material cmat = new(SubstanceManager.GetMaterial("AncientAlloy"))
            {
                name = "Copper (Cu)",
                color = new Color(1.00f,0.73f,0.20f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.AncientAlloy).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // Should be unique
            param.displayNameKey = "SUB_COPPER";
            param.material = cmat.name;
            param.density = 5.65f;
			param.strength = 90f;
			param.stiffness = 850f;
			param.hardness = 2.5f;
            param.thermalConductivity = 401f;
            param.softeningPoint = 600f;

            // Registering our substance as custom substance
            this.copper = CustomSubstanceManager.RegisterSubstance("Copper", param, new CustomSubstanceParams
            {
                enName = "Copper (Cu)",
                jpName = "Copper (Cu)",
                
            });
        }

        void RegisterIlmenite()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Stone"))
            {
                name = "Ilmenite (FeTiO3)",
                color = new Color(1f, 0.53f, 0.3f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.Stone).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // overwriteDeez
            param.displayNameKey = "SUB_ILMENITE";
            param.material = cmat.name;
            param.density = 5.65f;
			param.strength = 30f;
			param.stiffness = 400f;
			param.hardness = 5.6f;

            // Registering our substance as custom substance
            this.ilmenite = CustomSubstanceManager.RegisterSubstance("ilmenite", param, new CustomSubstanceParams
            {
                enName = "Ilmenite (FeTiO3)",
                jpName = "Ilmenite (FeTiO3)",
                
            });
        }

        void RegisterTitaniumoxide()
        {
            Material cmat = new(SubstanceManager.GetMaterial("QuartzSand"))
            
            {
                name = "Titanium oxide (TiO2)",
                color = new Color(0.97f,0.97f,1f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.Pyrite).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // overwriteDeez
            param.displayNameKey = "SUB_TITANIUMOXIDE";
            param.material = cmat.name;
            param.density = 4.9f;
			param.strength = 120f;
			param.stiffness = 120f;
			param.hardness = 5.6f;

            // Registering our substance as custom substance
            this.titaniumoxide = CustomSubstanceManager.RegisterSubstance("titaniumoxide", param, new CustomSubstanceParams
            {
                enName = "Titanium oxide (TiO2)",
                jpName = "Titanium oxide (TiO2)",
                behInit = delegate(CubeBase cb)
                {
                    // Adding test behavior
                    OreBeh oreBeh = cb.gameObject.AddComponent<OreBeh>();
                    oreBeh.refinedSubstance = this.titanium;
                    oreBeh.meltTemperature = 1400f;
                    return oreBeh;
                }
            });
        }

        void RegisterTitanium()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Tungsten"))
            {
                name = "Titanium (Ti)",
                color = new Color(0.70f,0.75f,0.71f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.Tungsten).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // overwriteDeez
            param.displayNameKey = "SUB_TITANIUM";
            param.material = cmat.name;
            param.density = 4.5f;
			param.strength = 100f;
			param.stiffness = 2000f;
			param.hardness = 6f;
            param.softeningPoint = 1200f;

            // Registering our substance as custom substance
            this.titanium = CustomSubstanceManager.RegisterSubstance("titanium", param, new CustomSubstanceParams
            {
                enName = "Titanium (Ti)",
                jpName = "Titanium (Ti)",
                
            });
        }

        void RegisterChromite()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Ice"))
            {
                name = "Chromite (FeCr2O4)",
                color = new Color(0.90f,1.00f,0.70f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.Pyrite).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // overwriteDeez
            param.displayNameKey = "SUB_CHROMITE";
            param.material = cmat.name;
            param.density = 5.0f;
			param.strength = 100f;
			param.stiffness = 90f;
			param.hardness = 5.4f;

            // Registering our substance as custom substance
            this.chromite = CustomSubstanceManager.RegisterSubstance("chromite", param, new CustomSubstanceParams
            {
                enName = "Chromite (FeCr2O4)",
                jpName = "Chromite (FeCr2O4)",
                
            });
        }

        void RegisterChromium()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Iron"))
            {
                name = "Chromium (Cr)",
                color = new Color(0.4f,0.6f,1f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.Iron).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // overwriteDeez
            param.displayNameKey = "SUB_CHROMIUM";
            param.material = cmat.name;
            param.density = 7.2f;
			param.strength = 130f;
			param.stiffness = 1700f;
			param.hardness = 8.5f;

            // Registering our substance as custom substance
            this.chromium = CustomSubstanceManager.RegisterSubstance("chromium", param, new CustomSubstanceParams
            {
                enName = "Chromium (Cr)",
                jpName = "Chromium (Cr)",
                
            });
        }

        void RegisterBauxite()
        {
            Material cmat = new(SubstanceManager.GetMaterial("QuartzSand"))
            {
                name = "Bauxite (Al(OH)3)",
                color = new Color(0.97f,0.51f,0.47f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.Pyrite).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // overwriteDeez
            param.displayNameKey = "SUB_BAUXITE";
            param.material = cmat.name;
            param.density = 3f;
			param.strength = 40f;
			param.stiffness = 20f;
			param.hardness = 1f;

            // Registering our substance as custom substance
            this.bauxite = CustomSubstanceManager.RegisterSubstance("bauxite", param, new CustomSubstanceParams
            {
                enName = "Bauxite (Al(OH)3)",
                jpName = "Bauxite (Al(OH)3)",
                behInit = delegate(CubeBase cb)
                {
                    // Adding test behavior
                    OreBeh oreBeh = cb.gameObject.AddComponent<OreBeh>();
                    oreBeh.refinedSubstance = this.aluminum;
                    oreBeh.meltTemperature = 850f;
                    return oreBeh;
                }
            });
        }

        void RegisterAluminum()
        {
            Material cmat = new(SubstanceManager.GetMaterial("AncientPlastic"))
            {
                name = "Aluminum (Al)",
                color = new Color(0.75f,0.76f,0.76f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.AncientPlastic).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // overwriteDeez
            param.displayNameKey = "SUB_ALUMINUM";
            param.material = cmat.name;
            param.density = 2.7f;
			param.strength = 40f;
			param.stiffness = 1200f;
			param.hardness = 2.75f;
            param.softeningPoint = 660f;

            // Registering our substance as custom substance
            this.aluminum = CustomSubstanceManager.RegisterSubstance("aluminum", param, new CustomSubstanceParams
            {
                enName = "Aluminum (Al)",
                jpName = "Aluminum (Al)",
                
            });
        }

        private void RegisterCoal()
		{
			Material material = new Material(SubstanceManager.GetMaterial("Stone"))
			{
				name = "Coal (C)",
				color = new Color(0.1f, 0.1f, 0.1f)
			};
			CustomMaterialManager.RegisterMaterial(material);
			SubstanceParameters.Param param = SubstanceManager.GetParameter(Substance.Wood).MemberwiseClone().Cast<SubstanceParameters.Param>();
			param.displayNameKey = "SUB_COAL";
			param.material = material.name;
			param.collisionSound = "stone1";
			param.density = 0.9f;
			param.combustionHeat = 21000f;
			param.combustionSpeed = 1E-05f;
			param.defaultPitch = 0.8f;
			param.strength = 20f;
			param.stiffness = 50f;
			param.hardness = 4f;
			this.coal = CustomSubstanceManager.RegisterSubstance("coal", param, new CustomSubstanceParams
			{
				enName = "Coal (C)",
				jpName = "Coal (C)"
			});
		}

        void RegisterSteel()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Ice"))
            {
                name = "Steel (Fe)",
                color = new Color(0.80f,0.8f,1f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.Iron).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // overwriteDeez
            param.displayNameKey = "SUB_STEEL";
            param.material = cmat.name;
            param.density = 7.8f;
			param.strength = 220f;
			param.stiffness = 200f;
			param.hardness = 6.2f;
            param.softeningPoint = 3000f;

            // Registering our substance as custom substance
            this.steel = CustomSubstanceManager.RegisterSubstance("steel", param, new CustomSubstanceParams
            {
                enName = "Steel (Fe)",
                jpName = "Steel (Fe)",
                
            });
        }

        void RegisterStainlessSteel()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Silver"))
            {
                name = "StainlessSteel (Fe)",
                color = new Color(0.70f,0.9f,1f)
            };
            // Registering material
            CustomMaterialManager.RegisterMaterial(cmat);

            // Getting substance params that our substance is based on and modifying them
            var param = SubstanceManager.GetParameter(Substance.Silver).MemberwiseClone().Cast<SubstanceParameters.Param>();

            // overwriteDeez
            param.displayNameKey = "SUB_STAINLESSSTEEL";
            param.material = cmat.name;
            param.density = 7.8f;
			param.strength = 310f;
			param.stiffness = 280f;
			param.hardness = 6.5f;
            param.softeningPoint = 5000f;

            // Registering our substance as custom substance
            this.stainlesssteel = CustomSubstanceManager.RegisterSubstance("stainlesssteel", param, new CustomSubstanceParams
            {
                enName = "StainlessSteel (Fe)",
                jpName = "StainlessSteel (Fe)",
                
            });
        }
        public override void OnUpdate()
        {
            // Spawning stuff above our head
            if (Input.GetKeyDown(KeyCode.L))
            {
                // Getting player position
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();

                // Generating the cube
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(-5f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), Substance.Tungsten);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(-4f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), Substance.Iron);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(-3f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.coal);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(-2f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.chalcocite);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(-1f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.ilmenite);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.titanium);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(1f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.chromite);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(2f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.chromium);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(3f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.titaniumoxide);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(4f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.bauxite);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(5f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), Substance.Sulfur);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(6f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.steel);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(7f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.stainlesssteel);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(7f, 5f, 1f), new Vector3(0.2f, 0.2f, 0.2f), this.copper);          
            }

            // Writing test data that will be stored in save file
            if (Input.GetKeyDown(KeyCode.X))
                ExtDataManager.SetData(new OurData(UnityEngine.Random.RandomRangeInt(1, 100)));
        }
    }
}