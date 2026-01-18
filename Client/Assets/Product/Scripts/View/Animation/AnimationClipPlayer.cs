using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Product.View.Animation
{
    public sealed class AnimationClipPlayer : IAnimationClipPlayer
    {
        readonly Animator animator;
        readonly AnimationClip[] animationClips;

        PlayableGraph playableGraph;

        CancellationTokenSource playCts;

        public AnimationClipPlayer(Animator animator, AnimationClip[] animationClips)
        {
            this.animator = animator;
            this.animationClips = animationClips;
        }

        public void PlayAnimation(bool isRestart, bool isEndEvaluate, Action onComplete)
        {
            PlayAnimationInner().Forget();

            async UniTask PlayAnimationInner()
            {
                await PlayAnimationAsync(isRestart, isEndEvaluate);
                onComplete?.Invoke();
            }
        }

        public async UniTask PlayAnimationAsync(bool isRestart, bool isEndEvaluate)
        {
            SetupContent();

            if (!playableGraph.IsValid())
            {
                return;
            }

            CancelCurrentPlay();

            var cts = new CancellationTokenSource();
            playCts = cts;
            var token = cts.Token;

            try
            {
                if (isRestart)
                {
                    ResetAnimationInternal();
                }

                playableGraph.Play();

                animator.enabled = true;
                await UniTask.WaitUntil(() => !playableGraph.IsValid() || playableGraph.IsDone(), cancellationToken: token);
                animator.enabled = false;

                if (isEndEvaluate && !token.IsCancellationRequested && ReferenceEquals(playCts, cts) && playableGraph.IsValid())
                {
                    playableGraph.Evaluate(float.MaxValue);
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                var isCurrent = ReferenceEquals(playCts, cts);
                if (isCurrent)
                {
                    if (playableGraph.IsValid())
                    {
                        playableGraph.Stop();
                    }

                    playCts = null;
                }

                cts.Dispose();
            }
        }

        public void ResetAnimation()
        {
            SetupContent();
            ResetAnimationInternal();
        }

        public void Stop()
        {
            CancelCurrentPlay();

            if (playableGraph.IsValid())
            {
                playableGraph.Stop();
            }
        }

        public void Skip()
        {
            SetupContent();
            CancelCurrentPlay();

            if (playableGraph.IsValid())
            {
                playableGraph.Evaluate(float.MaxValue);
                playableGraph.Stop();
            }
        }

        void CancelCurrentPlay()
        {
            if (playCts == null)
            {
                return;
            }

            var cts = playCts;
            playCts = null;
            cts.Cancel();
        }

        void SetupContent()
        {
            if (!playableGraph.IsValid())
            {
                CreatePlayableGraph(ref playableGraph, animator, animationClips);
            }
        }

        void ResetAnimationInternal()
        {
            if (!playableGraph.IsValid())
            {
                return;
            }

            SetTime(ref playableGraph, 0);
            playableGraph.Evaluate();
        }

        void OnDestroy()
        {
            CancelCurrentPlay();
            if (playableGraph.IsValid())
            {
                playableGraph.Destroy();
            }
        }

        static void CreatePlayableGraph(ref PlayableGraph playableGraph ,Animator animator, AnimationClip[] clips)
        {
            if (clips == null || clips.Length == 0)
            {
                return;
            }

            if (!playableGraph.IsValid())
            {
                playableGraph = PlayableGraph.Create(animator.transform.name);
            }

            var mixer = AnimationMixerPlayable.Create(playableGraph, clips.Length);
            var duration = 0f;

            for (var i = 0; i < clips.Length; i++)
            {
                var clip = clips[i];
                var playable = AnimationClipPlayable.Create(playableGraph, clip);
                mixer.ConnectInput(i, playable, 0, 1);
                duration = clip.length > duration ? clip.length : duration;
            }

            mixer.SetDuration(duration);
            AnimationPlayableOutput.Create(playableGraph, "output", animator).SetSourcePlayable(mixer);
            playableGraph.SetTimeUpdateMode(DirectorUpdateMode.UnscaledGameTime);
        }

        static void SetTime(ref PlayableGraph graph, float time)
        {
            var root = graph.GetRootPlayable(0);
            root.SetTime(time);
            root.SetDone(false);
            for (var i = 0; i < root.GetInputCount(); i++)
            {
                var playable = root.GetInput(i);
                playable.SetTime(time);
                playable.SetDone(false);
            }
        }
    }
}