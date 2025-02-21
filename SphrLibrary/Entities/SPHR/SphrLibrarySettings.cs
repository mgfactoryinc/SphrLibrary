using System.Runtime.Serialization;

namespace SphrLibrary.Entities.SPHR
{
    /// <summary>
    /// �ėp���W���[���ݒ�N���X��\���܂��B
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class SphrLibrarySettings
    {
        #region "Public Property"

        /// <summary>
        /// �T�[�r�XID���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DataMember()]
        public string ServiceId { get; set; } = string.Empty;

        /// <summary>
        /// �T�[�r�X�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DataMember()]
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// �X�g���[�W���[�g�p�X���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DataMember()]
        public string StorageRootPath { get; set; } = string.Empty;

        /// <summary>
        /// ���[�U�[ID�̌������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        /// <remarks>8���ȏ���w�肵�Ă��������B</remarks>
        [DataMember()]
        public int UserIdDigits { get; set; } = -1;

        #endregion

        #region "Constructor"

        /// <summary>
        /// <see cref="SphrLibrarySettings"/>�N���X�̐V�����C���X�^���X�����������܂��B
        /// </summary>
        public SphrLibrarySettings() { }

        #endregion

        #region "Public Method"

        /// <summary>
        /// �e�v���p�e�B�̗L���������؂��܂��B
        /// </summary>
        /// <returns>�S�ėL���Ȃ�true�A1�ł������Ȃ�false�B</returns>
        public bool IsValid() {
            return !string.IsNullOrWhiteSpace(ServiceId) && 
                !string.IsNullOrWhiteSpace(ServiceName) && 
                !string.IsNullOrWhiteSpace(StorageRootPath);
        }

        #endregion
    }
}