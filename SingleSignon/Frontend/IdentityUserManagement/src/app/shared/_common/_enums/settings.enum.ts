export enum SettingsType {
    DMS = 1,
    EMAIL = 2,
    REGULATION = 3,
    USER = 4
}

export enum SettingKeys {
    CaptchaSiteKey = 'CaptchaSiteKey',
    CaptchaSecretKey = 'CaptchaSecretKey',
    ProcessDeadline = 'ProcessDeadline',
    NewDiagnosisAlert = 'NewDiagnosisAlert',
    ProcessSLADeadline = 'ProcessSLADeadline',
    MaxFileSize = 'MaxFileSize',
    MaxUploadDate = 'MaxUploadDate',
    MaxUploadHour = 'MaxUploadHour'
}
