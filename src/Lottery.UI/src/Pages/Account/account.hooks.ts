import { useMutation, useQuery } from "@tanstack/react-query";

import { UpdateAccountResponse } from "./AccountDetails/schema";
import accountService from "./accountService";
import { ErrorResult, isReAuthError } from "../../services/ApiService";

interface UseGetAccountProps {
  enabled: boolean;
}

export function useGetAccount({ enabled }: UseGetAccountProps) {
  return useQuery({
    queryKey: ["account", "get"],
    queryFn: () => accountService.getAccount(),
    enabled,
  });
}

interface UseUpdateAccountProps {
  onSuccess(data: UpdateAccountResponse): void;
  onReAuthRequired(): void;
}

export function useUpdateAccount({
  onSuccess,
  onReAuthRequired,
}: UseUpdateAccountProps) {
  return useMutation({
    mutationFn: accountService.updateAccount,
    onSuccess,
    onError: (e: ErrorResult<unknown>) => {
      if (isReAuthError(e)) onReAuthRequired();
    },
  });
}

interface UseConfirmPasswordProps {
  onSuccess(): void;
  onFailure(): void;
}

export function useConfirmPassword({
  onFailure,
  onSuccess,
}: UseConfirmPasswordProps) {
  return useMutation({
    mutationFn: accountService.confirmPassword,
    onSuccess,
    onError: onFailure,
  });
}
