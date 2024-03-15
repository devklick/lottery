import { SignUpRequest, SignUpResponse } from "./SignUp/signUp.schema";
import { SignInRequest, SignInResponse } from "./SignIn/signIn.schema";
import { ApiService, ApiServiceDefinition } from "../services/ApiService";

interface AccountService {
  signIn(request: SignInRequest): Promise<SignInResponse>;
  signUp(request: SignUpRequest): Promise<SignUpResponse>;
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

    return result;
  };

  const signIn: AccountService["signIn"] = async (request) => {
    const result = await api.post<SignInRequest, SignInResponse>(
      "/account/signIn",
      request,
      { withCredentials: true }
    );

    if (result.success) return result.data;

    throw result.error;
  };

  return { signIn, signUp };
}

export default createAccountService({
  api: new ApiService({ baseUrl: "/lotteryapi" }),
});
