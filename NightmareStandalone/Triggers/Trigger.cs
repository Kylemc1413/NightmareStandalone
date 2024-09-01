using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareStandalone.Triggers {

    public enum Trigger {

        //Barriers
        BARRIER_CUT_START,      //TODO
        BARRIER_CUT_TICK,       //TODO
        BARRIER_CUT_END,        //TODO
        BARRIER_DAMAGE,         //TODO

        //Bombs //TODO ALL
        BOMB_CUT,
        BOMB_MISSED,

        //Controllers
        CONTROLLER_LEFT_TRIGGER_DOWN,
        CONTROLLER_LEFT_TRIGGER_UP,
        CONTROLLER_RIGHT_TRIGGER_DOWN,
        CONTROLLER_RIGHT_TRIGGER_UP,

        //Combo //TODO ALL
        COMBO_LOST,

        //Mode //TODO ALL
        MODE_DESELECTED,        
        MODE_SELECTED,          

        //Notes //TODO ALL
        NOTE_CUT_ANY,           
        NOTE_CUT_A,             
        NOTE_CUT_B,             
        NOTE_CUT_ALT_A,         
        NOTE_CUT_ALT_B,         
        NOTE_CUT_MONOCHROME,    
        NOTE_CUT_SUPER,

        NOTE_MISS_ANY,
        NOTE_MISS_A,
        NOTE_MISS_B,
        NOTE_MISS_ALT_A,
        NOTE_MISS_ALT_B,
        NOTE_MISS_MONOCHROME,
        NOTE_MISS_SUPER,

        NOTE_JUMP_FINISH,       //TODO

        //Powerups //TODO ALL
        POWERUP_CHARGED,
        POWERUP_END,
        POWERUP_START,

        //Sabers //TODO ALL
        SABER_COLLISION_START,      //TODO
        SABER_COLLISION_TICK,       //TODO
        SABER_COLLISION_END,        //TODO
        SABER_TOGGLE_TRIGGER_DOWN,  //TODO
        SABER_TOGGLE_TRIGGER_UP,    //TODO

        //Song
        SONG_END,               //TODO
        SONG_START, 

    }

    /* EVENTS
     * 
     * //Fuckery
     * Realtime Mirror          - {}                        - Nightmare mirroring effect
     * 
     * //Health
     * Heal/Harm                - {± float Amount}          - Add or Subtract from player health
     * Set                      - {float Amount}            - Set player health
     * 
     * //Powerups
     * Give Powerup             - {Powerup powerup}         - Provide the player a powerup
     * Give Powerup Charge      - {float amount}            - Provide the player charge for powerup system
     * 
     * //Sounds
     * Ambiance Start           - {string Filename}         - Start ambiance loop (like nightmare rain)
     * Ambiance Stop            - {}                        - Stops ambiance
     * Play Sound               - {string fileName}         - Play a sound effect once
     * 
     * //Visuals
     * Lightning Flash          - {bool isBackground}       - Lightning flash/sound ala Nightmare Mode
     * 
     */

}
