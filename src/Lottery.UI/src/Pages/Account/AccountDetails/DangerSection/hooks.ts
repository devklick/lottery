import { useMutation } from "@tanstack/react-query";

import { ErrorResult, isReAuthError } from "../../../../services/ApiService";
import accountService from "../../accountService";

interface UseDeleteAccountParams {
  onSuccess(): void;
  onReAuthRequired(): void;
}
export function useDeleteAccount({
  onReAuthRequired,
  onSuccess,
}: UseDeleteAccountParams) {
  return useMutation({
    mutationFn: accountService.deleteAccount,
    onSuccess,
    onError: (e: ErrorResult<unknown>) => {
      if (isReAuthError(e)) onReAuthRequired();
    },
  });
}
