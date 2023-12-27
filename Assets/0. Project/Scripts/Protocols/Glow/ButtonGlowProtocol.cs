using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BapelkesWebVrAnc.Protocols.Glow{

    /// <summary>
    /// Class ini berfungsi mengatur cara Glow periodically (Glow secara berkala)
    /// Khusus untuk Button
    /// </summary>
        
    public class ButtonGlowProtocol : ProtocolManager
    {

        private Color nonGlowingColor;

        private bool changedColor = false;
        [SerializeField] private Color32 startColor = new Color32(212, 201, 35, 255); // Warna awal
        [SerializeField] private Color32 targetColor = new Color32(78, 82, 61, 255); // Warna tujuan
        [SerializeField] private float glowingDuration; // Durasi lerp dalam detik
        private float glowTimer = 0f;
        private float glowChangeTimer = 0f;
        private bool isGlowing = false;
        private int numberForChangingColor = 0;
        private bool foreverGlow = false;

        [SerializeField] private Image myButton;

        void Start(){

            nonGlowingColor = myButton.color;
            
        }

        void Update(){

            
            if (isGlowing){

                //Mengganti Color Button ke Glow material
                if (!changedColor){
                    changedColor = true;
                }
                    

                // Menghitung nilai lerp antara startColor dan targetColor berdasarkan waktu
                float lerpValue = Mathf.Clamp01(glowChangeTimer / 0.5f);

                if (numberForChangingColor == 0){

                    // Menggunakan lerpValue untuk mengubah warna secara perlahan
                    Color lerpedColor = Color.Lerp(startColor, targetColor, lerpValue);

                    // Mengatur warna pada renderer
                    ChangeMyButtonColor(lerpedColor);
                }

                else if (numberForChangingColor == 1){

                    // Menggunakan lerpValue untuk mengubah warna secara perlahan
                    Color lerpedColor = Color.Lerp(targetColor, startColor, lerpValue);

                    // Mengatur warna pada renderer
                    ChangeMyButtonColor(lerpedColor);
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
            ChangeMyButtonColor(nonGlowingColor);
        }

        //============================Changing Materials or Materials Properties=========================================

        //Method untuk mengganti Material MyRenderer dengan Material yg sama
        void ChangeMyButtonColor(Color32 color){
            Debug.Log("Mestinya terganti yah Colornya");
            myButton.color = color;
        }

        //===============================OVERRIDES FUNCTION===============================
        public override void StartTheProtocol()
        {
            if (glowingDuration == 0)
                StartToGlow(0);
            else
                StartToGlow(glowingDuration);
                
            changedColor = false;

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
