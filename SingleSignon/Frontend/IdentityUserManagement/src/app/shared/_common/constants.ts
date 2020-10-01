import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

export class Constants {

  // Formaters
  static readonly DATE_FMT = 'dd/MMM/yyyy';
  static readonly DATE_NUM_FMT = 'dd/MM/yyyy';
  static readonly DATE_TIME_FMT = `${Constants.DATE_FMT} hh:mm:ss`;
  static readonly DATE_NUM_TIME_FMT = `${Constants.DATE_NUM_FMT} HH:mm:ss`;
  static readonly SYSTEM_DATE_FMT = 'yyyy-MM-dd';
  static readonly SYSTEM_DATE_TIME_FMT = `${Constants.SYSTEM_DATE_FMT} hh:mm:ss`;
  static readonly PASSWORD_SYMBOL = '*';

  // tslint:disable-next-line:max-line-length
  static readonly bsConfig: Partial<BsDatepickerConfig> = Object.assign({}, { containerClass: 'theme-blue', dateInputFormat: 'DD/MM/YYYY' });

  // URL

  static readonly APIEndpoint = 'http://localhost/filegestapi/api/';
  static readonly APISignalREndpoint = 'http://localhost/filegestapi/';

   //static readonly APIEndpoint = 'http://webapi.descobrirsps.lan/api/';
   //static readonly APISignalREndpoint = 'http://webapi.descobrirsps.lan/';


  // Settings
  static readonly AppContacts = { Phone: '217783839', PhoneSpaced: '217 783 839' };
  static readonly AppEmailEnum = { Support: 1, Comercial: 2 };
  static readonly AppEmailEnumString = { Support: 'joao.almeida.viseu@outlook.pt' };
  // static readonly SupportEmail = 'gdpr.test@pessoaseprocessos.com';
  // static readonly ComercialEmail = 'gdpr.test@pessoaseprocessos.com';

  // user login
  static readonly ActivationTypeEnum = { Activation: 1, Recovery: 2, ReactivateRequest: 3, UsernameRecovery: 4, AccountRecovery: 5 };

  // Variables
  static PAGE_SIZE = 12;
  static DESIGNATION_MAX_LENGTH = 50;
  static DESCRIPTION_MAX_LENGTH = 200;
  static VOTES_MANDATORY = 3;

  // ActiveEnum
  static readonly ActiveTypes = { Yes: true, No: false };

  // Logging
  static readonly LogTypes = { Error: 1, Debug: 2, Information: 3, Warning: 4 };
  static readonly LogActivityTypes = {
    Login: 1, CreateAction: 2, DeleteAction: 3,
    UpdateAction: 4, ErrorAction: 5, UserActivation: 6,
    PasswordRecovery: 7, UserReactivation: 8, PublishAction: 9,
    FinishAction: 10, ReassignAction: 11
  };

  // states
  static readonly State = { Active: true, Inactive: false };

  // diagnosis
  static readonly ComplianceStatus = { NotVerified: 1, NotApplyable: 2, Compliant: 3, NonCompliant: 4 };
  static readonly ComplianceType = { Recomendation: 1, Improvement: 2 };
  static readonly ComplianceTranslationType = { Recomendation: 1, Improvement: 2 };
  static readonly DiagnosisStatus = { Started: 1, Finished: 10 };

  // processes
  static readonly ProcessType = { RecomendationVerify: 1, ImproveVerify: 2 };
  static readonly ProcessComplianceType = { Recomendation: 1, Improvement: 2 };
  static readonly ProcessStatus = { Registered: 1, CompletedSuccess: 2, CompletedWithoutSuccess: 3 };

  // disclaimers
  static readonly DisclaimerType = { TermsOfService: 1, PrivacyPolicy: 2 };
  static readonly SettingsTypeEnum = { DMS: 1, EMAIL: 2, REGULATION: 3, USER: 4 };

  static readonly QuillOptions = {
    toolbar: [
      [{ 'size': ['small', false, 'large'] }],  // custom dropdown - [{ 'size': ['small', false, 'large', 'huge'] }],

      ['bold', 'italic', 'underline', 'strike'],
      // ['blockquote', 'code-block'],
      // [{ header: 1 }, { header: 2 }],
      [{ list: 'ordered' }, { list: 'bullet' }],
      // [{ script: 'sub' }, { script: 'super' }],
      [{ indent: '-1' }, { indent: '+1' }],
      // [{ direction: 'rtl' }],
      // [{ header: [1, 2, 3, 4, 5, 6, false] }],
      // [{ 'color': [] }, { 'background': [] }],
      ['clean'], // remove formatting button
      [] // link and image, video - ['link', 'image', 'video']
    ]
  };

  static readonly QuillOptionsExtra = {
    toolbar: [
      [{ header: [1, 2, 3, 4, 5, 6, false] }],
      [{ 'size': ['small', false, 'large', 'huge'] }],

      ['bold', 'italic', 'underline', 'strike'],
      // ['blockquote', 'code-block'],
      // [{ header: 1 }, { header: 2 }],
      [{ list: 'ordered' }, { list: 'bullet' }],
      // [{ script: 'sub' }, { script: 'super' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ 'align': [] }],
      // [{ direction: 'rtl' }],
      // [{ 'color': [] }, { 'background': [] }],
      ['clean'], // remove formatting button
      ['link'] // link and image, video - ['link', 'image', 'video']
    ]
  };

  // file extensions valid
  static readonly evidenceDocExtensions = ['pdf', 'png', 'jpg', 'jpeg', 'gif'];

  // payments
  static readonly paymentIban = 'PT50 0018 0003 1387 3070 0202 5';
  static readonly paymentValue = '664.20';
  static readonly paymentValueTaxFree = '540.00';

}
