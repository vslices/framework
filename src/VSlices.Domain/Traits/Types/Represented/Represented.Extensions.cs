namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class RepresentedExtensions
{
    extension<REPR>(REPR self)
        where REPR : Represented<REPR>
    {
        /// <summary>
        ///
        /// </summary>
        public REPR To() => self.To();
    }
}
