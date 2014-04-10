
using Mogre;

namespace Mogre_Procedural.Game.BaseApp
{
    public class RootLstn
    {
        public enum TypeLstn { FrameStarted, FrameRendering, FrameEnded }
        public delegate bool FrameLstn(FrameEvent evt);

        private readonly TypeLstn mType;
        private readonly FrameLstn mListener;

        public RootLstn(TypeLstn type, FrameLstn listener) {
            this.mType = type;
            this.mListener = listener;
        }

        public void AddListener(Root root) {
            if (this.mType == TypeLstn.FrameStarted) { root.FrameStarted += new Mogre.FrameListener.FrameStartedHandler(this.mListener); }
            else if (this.mType == TypeLstn.FrameRendering) { root.FrameRenderingQueued += new Mogre.FrameListener.FrameRenderingQueuedHandler(this.mListener); }
            else /* this.mType == TypeListener.FrameEnded */ { root.FrameEnded += new Mogre.FrameListener.FrameEndedHandler(this.mListener); }
        }

        public void RemoveListener(Root root) {
            if (this.mType == TypeLstn.FrameStarted) { root.FrameStarted -= new Mogre.FrameListener.FrameStartedHandler(this.mListener); }
            else if (this.mType == TypeLstn.FrameRendering) { root.FrameRenderingQueued -= new Mogre.FrameListener.FrameRenderingQueuedHandler(this.mListener); }
            else /* this.mType == TypeListener.FrameEnded */ { root.FrameEnded -= new Mogre.FrameListener.FrameEndedHandler(this.mListener); }
        }
    }


}