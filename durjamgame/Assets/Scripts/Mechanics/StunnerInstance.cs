﻿using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

public class StunnerInstance : MonoBehaviour
{
        public AudioClip stunconsumeCollectAudio;
        [Tooltip("If true, animation will start at a random position in the sequence.")]
        public bool randomAnimationStartTime = false;
        [Tooltip("List of frames that make up the animation.")]
        public Sprite[] idleAnimation, collectedAnimation;

        internal Sprite[] sprites = new Sprite[0];

        internal SpriteRenderer _renderer;
        internal Animator animator;

        //active frame in animation, updated by the controller.
        internal int frame = 0;

        public float cycleDelay;
        public float cycleDuration;
        private float currentTime;
        private float lastCycle;


        private bool activated = false;

        void ToggleActivated()
        {
            // Debug.Log(activated);
            activated = !activated;
            // Handle activation logic here.
            // Debug.Log(activated);
            animator.SetBool("activated", activated);

        }

        void Awake()
        {
            animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            if (randomAnimationStartTime)
                frame = Random.Range(0, sprites.Length);
            sprites = idleAnimation;
            currentTime = -cycleDelay;
            lastCycle = -cycleDuration;
        }

        void Update()
        {
            // Debug.Log(currentTime);
            // Debug.Log(lastCycle);
            currentTime += Time.deltaTime;
            if (currentTime > lastCycle + cycleDuration)
            {
                lastCycle += cycleDuration;
                ToggleActivated();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (activated)
            {
                //only exectue OnPlayerEnter if the player collides with this stunconsume.
                var player = other.gameObject.GetComponent<PlayerController>();
                if (player != null) OnPlayerEnter(player);
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (activated)
            {
                //only exectue OnPlayerEnter if the player collides with this stunconsume.
                var player = other.gameObject.GetComponent<PlayerController>();
                if (player != null) OnPlayerEnter(player);
            }
        }

        void OnPlayerEnter(PlayerController player)
        {
            //disable the gameObject and remove it from the controller update list.
            frame = 0;
            sprites = collectedAnimation;
            //send an event into the gameplay system to perform some behaviour.
            var ev = Schedule<PlayerStunCollision>();
            ev.player = player;
        }
}
