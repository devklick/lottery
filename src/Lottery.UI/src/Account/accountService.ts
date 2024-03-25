import {
  SignUpRequest,
  SignUpResponse,
  signUpResponseSchema,
} from "./SignUp/signUp.schema";
import {
  SignInRequest,
  SignInResponse,
  signInResponseSchema,
} from "./SignIn/signIn.schema";
import { ApiService, ApiServiceDefinition } from "../services/ApiService";

interface AccountService {
  signIn(request: SignInRequest): Promise<SignInResponse>;
  signUp(request: SignUpRequest): Promise<SignUpResponse>;
  signOut(): Promise<void>;
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
      { withCredentials: true }
    );

    if (!result.success) {
      throw result.error;
    }

    const valid = signUpResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const signIn: AccountService["signIn"] = async (request) => {
    const result = await api.post<SignInRequest, SignInResponse>(
      "/account/signIn",
      request,
      { withCredentials: true }
    );

    if (!result.success) {
      throw result.error;
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

  return { signIn, signUp, signOut };
}

export default createAccountService({
  api: new ApiService({ baseUrl: "/lotteryapi" }),
});
