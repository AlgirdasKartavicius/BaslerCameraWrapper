namespace CoreCamera
{
    public interface ICameraCore
    {
        /// <summary>
        /// Is camera currently grabbing
        /// </summary>
        bool IsGrabbing { get; set; }

        /// <summary>
        /// Set camera exposure time
        /// </summary>
        /// <param name="exposure">exposure value</param>
        void SetExposure(long exposure);

        /// <summary>
        /// Set camera gain parameter
        /// </summary>
        /// <param name="gain">gain value</param>
        void SetGain(long gain);

        /// <summary>
        /// Set frame height
        /// </summary>
        /// <param name="height">height value</param>
        void SetHeight(long height);

        /// <summary>
        /// Set frame width
        /// </summary>
        /// <param name="width">width value</param>
        void SetWidth(long width);

        /// <summary>
        /// Start frames grabbing
        /// </summary>
        void Grab();

        /// <summary>
        /// Stop frames grabbing
        /// </summary>
        void StopGrab();

        /// <summary>
        /// Write current settings to camera device
        /// </summary>
        void UpdateSettings();

        /// <summary>
        /// Terminate basler pylon
        /// </summary>
        void Terminate();
    }
}