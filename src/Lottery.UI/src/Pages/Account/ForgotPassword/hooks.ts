import { useMutation } from "@tanstack/react-query";

import accountService from "../accountService";

interface UseForgotPasswordParams {
  onSuccess: VoidFunction;
}
export function useForgotPassword({ onSuccess }: UseForgotPasswordParams) {
  return useMutation({
    mutationFn: accountService.forgotPassword,
    onSuccess,
  });
}
