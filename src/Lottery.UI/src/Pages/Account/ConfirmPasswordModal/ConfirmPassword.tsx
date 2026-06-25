import { Button, PasswordInput, Stack, Text } from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";

import {
  ConfirmPasswordRequestBody,
  confirmPasswordRequestBodySchema,
} from "./confirmPassword.schema";
import { useConfirmPassword } from "../account.hooks";

interface ConfirmPasswordProps {
  reason: string;
  onSuccess(): void;
  onFailure(): void;
}
export default function ConfirmPassword({
  reason,
  onFailure,
  onSuccess,
}: ConfirmPasswordProps) {
  const form = useForm<ConfirmPasswordRequestBody>({
    validate: schemaResolver(confirmPasswordRequestBodySchema),
  });

  const confirmPassword = useConfirmPassword({ onFailure, onSuccess });
  return (
    <form
      id="confirm-password"
      onSubmit={form.onSubmit(
        async (data) => await confirmPassword.mutateAsync(data),
      )}
    >
      <Stack>
        <Text>{reason}</Text>
        <PasswordInput
          {...form.getInputProps("password")}
          name="password"
          type="password"
        />
        <Button type="submit" id="confirm-password">
          Submit
        </Button>
      </Stack>
    </form>
  );
}
