import {
  UpdateAccountRequestBody,
  UpdateAccountResponse,
  updateAccountResponseSchema,
} from "./AccountDetails/updateAccountDetails.schema";
import {
  ConfirmEmailChangeRequestQuery,
  ConfirmEmailChangeResponse,
  confirmEmailChangeResponseSchema,
} from "./ConfirmEmailChange/schema";
import {
  ConfirmPasswordRequestBody,
  ConfirmPasswordResponse,
  confirmPasswordResponseSchema,
} from "./ConfirmPasswordModal/confirmPassword.schema";
import {
  ForgotPasswordRequestBody,
  ForgotPasswordResponse,
} from "./ForgotPassword/schema";
import {
  ResetPasswordRequestBody,
  ResetPasswordResponse,
} from "./ResetPassword/schema";
import {
  SignInRequest,
  SignInResponse,
  signInResponseSchema,
} from "./SignIn/signIn.schema";
import {
  GetAccountResponse,
  getAccountResponseSchema,
} from "./SignUp/getAccount.schema";
import {
  SignUpRequest,
  signUpResponseSchema,
  SignUpResponse,
} from "./SignUp/signUp.schema";
import {
  VerifyEmailRequestQuery,
  VerifyEmailResponse,
  verifyEmailResponseSchema,
} from "./VerifyEmail/schema";
import { ApiService, ApiServiceDefinition } from "../../services/ApiService";
import { DeleteAccountResponse } from "./AccountDetails/DangerSection/schema";

interface AccountService {
  signIn(request: SignInRequest): Promise<SignInResponse>;
  signUp(request: SignUpRequest): Promise<SignUpResponse>;
  signOut(): Promise<void>;
  getAccount(): Promise<GetAccountResponse>;
  updateAccount(
    request: UpdateAccountRequestBody,
  ): Promise<UpdateAccountResponse>;
  confirmPassword(
    request: ConfirmPasswordRequestBody,
  ): Promise<ConfirmPasswordResponse>;
  verifyEmail(request: VerifyEmailRequestQuery): Promise<VerifyEmailResponse>;
  confirmEmailChange(
    request: ConfirmEmailChangeRequestQuery,
  ): Promise<ConfirmEmailChangeResponse>;
  forgotPassword(
    request: ForgotPasswordRequestBody,
  ): Promise<ForgotPasswordResponse>;
  resetPassword(
    request: ResetPasswordRequestBody,
  ): Promise<ResetPasswordResponse>;
  deleteAccount(): Promise<DeleteAccountResponse>;
}

export function createAccountService({
  api,
}: {
  api: ApiServiceDefinition;
}): AccountService {
  const signUp: AccountService["signUp"] = async (request) => {
    const result = await api.post<SignUpRequest, SignUpResponse>(
      "/account/signUp",
      request,
      { withCredentials: true },
    );

    if (!result.success) {
      throw result;
    }

    const valid = signUpResponseSchema.safeParse(result.data);
    console.log("sign up response", valid, result);

    if (valid.success) {
      return valid.data.value;
    }

    throw valid.error.message;
  };

  const signIn: AccountService["signIn"] = async (request) => {
    const result = await api.post<SignInRequest, SignInResponse>(
      "/account/signIn",
      request,
      { withCredentials: true },
    );

    if (!result.success) {
      throw result;
    }

    const valid = signInResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const signOut: AccountService["signOut"] = async () => {
    await api.post("/account/signOut", undefined, {
      withCredentials: true,
    });
  };
  const getAccount: AccountService["getAccount"] = async () => {
    const result = await api.get<null, GetAccountResponse>("/account", null, {
      withCredentials: true,
    });

    if (!result.success) {
      throw result.errors;
    }

    const valid = getAccountResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const updateAccount: AccountService["updateAccount"] = async (request) => {
    const result = await api.post<
      UpdateAccountRequestBody,
      UpdateAccountResponse
    >("/account", request, {
      withCredentials: true,
    });

    if (!result.success) {
      throw result;
    }

    const valid = updateAccountResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const confirmPassword: AccountService["confirmPassword"] = async (
    request,
  ) => {
    const result = await api.post<
      ConfirmPasswordRequestBody,
      ConfirmPasswordResponse
    >("/account/password/confirm", request, {
      withCredentials: true,
    });

    if (!result.success) {
      throw result;
    }

    const valid = confirmPasswordResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const confirmEmail: AccountService["verifyEmail"] = async (request) => {
    const result = await api.get<VerifyEmailRequestQuery, VerifyEmailResponse>(
      "/account/email/confirm",
      request,
    );

    if (!result.success) {
      throw result;
    }

    const valid = verifyEmailResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const confirmEmailChange: AccountService["confirmEmailChange"] = async (
    request,
  ) => {
    const result = await api.get<VerifyEmailRequestQuery, VerifyEmailResponse>(
      "/account/email/confirmChange",
      request,
    );

    if (!result.success) {
      throw result;
    }

    const valid = confirmEmailChangeResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const forgotPassword: AccountService["forgotPassword"] = async (request) => {
    const result = await api.post<
      ForgotPasswordRequestBody,
      ForgotPasswordResponse
    >("/account/password/forgot", request);

    if (!result.success) {
      throw result;
    }

    const valid = confirmEmailChangeResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const resetPassword: AccountService["resetPassword"] = async (request) => {
    const result = await api.post<
      ResetPasswordRequestBody,
      ResetPasswordResponse
    >("/account/password/reset", request);

    if (!result.success) {
      throw result;
    }

    const valid = confirmEmailChangeResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const deleteAccount: AccountService["deleteAccount"] = async () => {
    const result = await api.delete<unknown, DeleteAccountResponse>("/account");

    console.log("result", result);
    if (!result.success) {
      throw result;
    }

    const valid = confirmEmailChangeResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  return {
    signIn,
    signUp,
    signOut,
    getAccount,
    updateAccount,
    confirmPassword,
    verifyEmail: confirmEmail,
    confirmEmailChange,
    forgotPassword,
    resetPassword,
    deleteAccount,
  };
}

export default createAccountService({
  api: new ApiService({ baseUrl: "/lotteryapi" }),
});
