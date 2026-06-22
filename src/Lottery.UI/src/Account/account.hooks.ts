import { useMutation, useQuery } from "@tanstack/react-query";

import accountService from "./accountService";
import { apiMessagesSchema } from "../common/schemas";
import { ErrorResult } from "../services/ApiService";
import { UpdateAccountResponse } from "./AccountDetails/updateAccountDetails.schema";

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
      console.log("useUpdateAccount error", e);
      const validation = apiMessagesSchema.safeParse(e.errors);
      console.log("useUpdateAccount error", e, validation);
      if (
        validation.success &&
        validation.data[0].code === "RecentAuthRequired"
      ) {
        onReAuthRequired();
      }
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
