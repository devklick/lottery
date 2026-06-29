import {
  Button,
  ButtonGroup,
  Collapse,
  Group,
  PasswordInput,
  Stack,
} from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";

import { useChangePassword } from "./hooks";
import { ChangePasswordForm, changePasswordFormSchema } from "./schema";
import FieldWrapper from "../../../components/FieldWrapper";

export default function ChangePassword() {
  const [expanded, { toggle }] = useDisclosure();

  const form = useForm<ChangePasswordForm>({
    validate: schemaResolver(changePasswordFormSchema),
    validateInputOnChange: true,
  });

  const changePassword = useChangePassword();

  return (
    <Stack w="100%" flex="1 1 0">
      <Collapse expanded={expanded}>
        <form
          id="change-password-form"
          onSubmit={form.onSubmit(
            async (data) => await changePassword.mutateAsync(data),
          )}
        >
          <Stack>
            <FieldWrapper
              name="Current Password"
              required
              description="You must know your current password in order to change it"
            >
              <PasswordInput
                {...form.getInputProps("currentPassword")}
                disabled={!expanded}
              />
            </FieldWrapper>
            <FieldWrapper
              name="New Password"
              required
              description="This is the password you want to use in the future"
            >
              <PasswordInput
                {...form.getInputProps("newPassword")}
                disabled={!expanded}
              />
            </FieldWrapper>
            <FieldWrapper
              name="Confirm New Password"
              required
              description="Re-type your new password to ensure you have entered correctly"
            >
              <PasswordInput
                {...form.getInputProps("confirmNewPassword")}
                disabled={!expanded}
              />
            </FieldWrapper>
          </Stack>
        </form>
      </Collapse>
      <Group justify="end">
        <ButtonGroup>
          {expanded && (
            <>
              <Button onClick={toggle} variant="outline" color="red">
                Cancel
              </Button>
              <Button
                type="submit"
                variant="gradient"
                gradient={{ from: "blue", to: "grape", deg: 20 }}
                form={"change-password-form"}
              >
                Submit
              </Button>
            </>
          )}
          {!expanded && <Button onClick={toggle}>Change Password</Button>}
        </ButtonGroup>
      </Group>
    </Stack>
  );
}
