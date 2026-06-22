import { Button, Modal, PasswordInput, Stack } from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";

import {
  ConfirmPasswordRequestBody,
  confirmPasswordRequestBodySchema,
} from "./confirmPassword.schema";
import { useConfirmPassword } from "../account.hooks";

interface ConfirmPasswordModalProps {
  onSuccess(): void;
  onFailure(): void;
  onCancel(): void;
  opened: boolean;
}

export default function ConfirmPasswordModal({
  onCancel,
  onFailure,
  onSuccess,
  opened,
}: ConfirmPasswordModalProps) {
  const form = useForm<ConfirmPasswordRequestBody>({
    validate: schemaResolver(confirmPasswordRequestBodySchema),
  });

  const confirmPassword = useConfirmPassword({ onFailure, onSuccess });

  return (
    <Modal
      title="Confirm Password"
      opened={opened}
      // withCloseButton
      onClose={onCancel}
    >
      <form
        id="confirm-password"
        onSubmit={form.onSubmit(
          async (data) => await confirmPassword.mutateAsync(data),
        )}
      >
        <Stack>
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
    </Modal>
  );
}
