import { useMutation } from "@tanstack/react-query";

import accountService from "../accountService";

interface UseResetPasswordParams {
  onSuccess: VoidFunction;
}
export function useResetPassword({ onSuccess }: UseResetPasswordParams) {
  return useMutation({
    mutationFn: accountService.resetPassword,
    onSuccess,
  });
}
