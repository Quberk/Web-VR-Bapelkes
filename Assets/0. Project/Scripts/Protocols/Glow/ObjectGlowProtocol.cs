using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.Protocols.Glow{

    /// <summary>
    /// Class ini berfungsi mengatur cara Glow periodically (Glow secara berkala)
    /// </summary>

    public class ObjectGlowProtocol : ProtocolManager
    {
        ///<param name = "nonGlowingMaterial">Material Awal sebelum terjadi Glow</param>
        ///<param name = "glowingMaterial">Material Glow</param>
        ///<param name = "changedMaterial">Bool untuk mengecek apakah Material sudah terganti</param>
        ///<param name = "startColor">Referensi warna awal Glow</param>
        ///<param name = "targetColor">Referensi warna akhir Glow</param>
        ///<param name = "glowingDuration">Durasi Glowing</param>
        ///<param name = "glowTimer">Patokan waktu yang terus bertambah hingga mencapai glowingDuration</param>
        ///<param name = "glowChangeTimer">Patokan waktu jika warna berganti dari warna 1 ke warna lainnya</param>
        ///<param name = "isGlowing">Untuk mengetahui bahwa sudah saatnya Glow</param>
        ///<param name = "numberForChangingColor">1 sebagai perubahan warna dari Start ke Target dan 2 sebaliknya</param>
        ///<param name = "foreverGlow">Sebagai referensi untuk mengetahui apakah Glow akan terus dieksekusi tanpa batas tertentu</param>
        ///<param name = "myRenderer">Referensi Renderer Komponen pada Objek yg ingin diGlow</param>
        ///<param name = "material">Referensi Material Komponen pada Objek yg ingin diGlow</param>

        private Material[] nonGlowingMaterial;
        [SerializeField] private Material glowingMaterial;

        private bool changedMaterial = false;
        [SerializeField] private Color startColor = new Color32(212, 201, 35, 0); // Warna awal
        [SerializeField] private Color targetColor = new Color32(78, 82, 61, 0); // Warna tujuan
        [SerializeField] private float glowingDuration; // Durasi lerp dalam detik
        private float glowTimer = 0f;
        private float glowChangeTimer = 0f;
        private bool isGlowing = false;
        private int numberForChangingColor = 0;
        private bool foreverGlow = false;

        [SerializeField] private MeshRenderer[] myRenderer;
        [SerializeField] private SkinnedMeshRenderer[] mySkinnedMeshRenderer;

        void Start(){

            nonGlowingMaterial = new Material[myRenderer.Length + mySkinnedMeshRenderer.Length];


            for(int i = 0; i < myRenderer.Length; i++){
                nonGlowingMaterial[i] = myRenderer[i].material;
            }

            for(int i = myRenderer.Length; i < (myRenderer.Length + mySkinnedMeshRenderer.Length); i++){
                nonGlowingMaterial[i] = mySkinnedMeshRenderer[i].material;
            }
            
        }

        void Update(){
        
            if (isGlowing){

                //Mengganti material ke Glow material
                if (!changedMaterial){
                    ChangeMyRenderersMaterial(glowingMaterial);
                    changedMaterial = true;
                }
                    

                // Menghitung nilai lerp antara startColor dan targetColor berdasarkan waktu
                float lerpValue = Mathf.Clamp01(glowChangeTimer / 0.5f);

                if (numberForChangingColor == 0){

                    // Menggunakan lerpValue untuk mengubah warna secara perlahan
                    Color lerpedColor = Color.Lerp(startColor, targetColor, lerpValue);

                    // Mengatur warna pada renderer
                    ChangeMaterialColor(lerpedColor);
                }

                else if (numberForChangingColor == 1){

                    // Menggunakan lerpValue untuk mengubah warna secara perlahan
                    Color lerpedColor = Color.Lerp(targetColor, startColor, lerpValue);

                    // Mengatur warna pada renderer
                    ChangeMaterialColor(lerpedColor);
                }

                //Switch Time
                if (glowChangeTimer >= 0.5f)
                {
                    glowChangeTimer = 0;

                    if (numberForChangingColor == 0)
                        numberForChangingColor = 1;
                    else
                        numberForChangingColor = 0;
                }

                // Menambahkan waktu ke timer
                glowChangeTimer += Time.deltaTime;
                glowTimer += Time.deltaTime;

                // Menghentikan lerp jika sudah mencapai durasi yang ditentukan dan jika Forever Glow False
                if (glowTimer >= glowingDuration && !foreverGlow)
                {
                    StopTheProtocol();
                }
            }
        }

        public void StartToGlow(float glowingDuration){
            glowTimer = 0f;
            isGlowing = true;

            this.glowingDuration = glowingDuration;

            if (glowingDuration == 0){
                foreverGlow = true;
            }
        }

        public void StopToGlow(){
            isGlowing = false;
            ChangeMyRenderersMaterial(nonGlowingMaterial);
        }

        //============================Changing Materials or Materials Properties=========================================

        //Method untuk mengganti Material MyRenderer dengan Material yg sama
        void ChangeMyRenderersMaterial(Material material){

            for (int i = 0; i < myRenderer.Length; i++){
                myRenderer[i].material = material;
            }

            for (int i = 0; i < mySkinnedMeshRenderer.Length; i++){
                mySkinnedMeshRenderer[i].material = material;
            }
        }

        //Method untuk mengganti Material MyRenderer dengan Material yg berbeda-beda
        void ChangeMyRenderersMaterial(Material[] materials){
            
            for (int i = 0; i < myRenderer.Length; i++){
                myRenderer[i].material = materials[i];
            }

            for (int i = 0; i < mySkinnedMeshRenderer.Length; i++){
                mySkinnedMeshRenderer[i].material = materials[i];
            }
        }

        //Method untuk mengganti Color Property pada Material
        void ChangeMaterialColor(Color color){
            for (int i = 0; i < myRenderer.Length; i++){
                myRenderer[i].material.SetColor("_EmissionColor", color);
            }

            for (int i = 0; i < mySkinnedMeshRenderer.Length; i++){
                mySkinnedMeshRenderer[i].material.SetColor("_EmissionColor", color);
            }
        }

        //===============================OVERRIDES FUNCTION===============================
        public override void StartTheProtocol()
        {
            if (glowingDuration == 0)
                StartToGlow(0);
            else
                StartToGlow(glowingDuration);
                
            changedMaterial = false;

            protocolStarted = true;
        }

        public override void StopTheProtocol()
        {
            StopToGlow();
            protocolFinished = true;
            protocolStarted = false;
        }
    }
}

