import { Divider, Stack, TextInput } from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";
import { IconCheck, IconExclamationMark } from "@tabler/icons-react";
import { useCallback, useState } from "react";

import ChangePassword from "./ChangePassword";
import DangerSection from "./DangerSection/DangerSection";
import EditButton from "./EditButton";
import {
  UpdateAccountRequestBody,
  updateAccountRequestBodySchema,
} from "./schema";
import { useWaitFor } from "../../../common/hooks/time.hooks";
import { notifyError } from "../../../common/notifications";
import FieldWrapper from "../../../components/FieldWrapper";
import PageSection from "../../../components/PageSection";
import { useGetAccount, useUpdateAccount } from "../account.hooks";
import ConfirmPasswordModal from "../ConfirmPasswordModal";
import { GetAccountResponse } from "../SignUp/getAccount.schema";

interface AccountDetailsProps {
  authenticated: boolean;
}

export default function AccountDetails({ authenticated }: AccountDetailsProps) {
  const [editing, setEditing] = useState(false);

  const form = useForm<UpdateAccountRequestBody>({
    validate: schemaResolver(updateAccountRequestBodySchema),
    validateInputOnChange: true,
  });

  const { initialize: initializeForm } = form;

  const onQuerySuccess = useCallback(
    (data: GetAccountResponse) => {
      initializeForm({
        email: data.email ?? "",
        phoneNumber: data.phoneNumber ?? "",
        username: data.username ?? "",
      });
    },
    [initializeForm],
  );

  const accountQuery = useGetAccount({
    enabled: authenticated,
    onSuccess: onQuerySuccess,
  });

  const loading = useWaitFor(accountQuery.isLoading, 400);

  const [
    confirmPasswordModalOpened,
    { open: openConfirmPasswordModal, close: closeConfirmPasswordModal },
  ] = useDisclosure(false);

  const updateAccount = useUpdateAccount({
    onSuccess: updateAccountSuccess,
    onReAuthRequired: openConfirmPasswordModal,
  });

  async function updateAccountSuccess() {
    closeConfirmPasswordModal();
    setEditing(false);
    await accountQuery.refetch();
  }

  async function confirmPasswordSuccess() {
    await updateAccount.mutateAsync(form.getValues());
  }

  function confirmPasswordFailed() {
    notifyError({
      title: "Password verification failed",
      message: "Unable to update your account details",
    });

    closeConfirmPasswordModal();
    form.reset();
    setEditing(false);
  }

  return (
    <PageSection
      title="Account Details"
      collapsable
      width={"100%"}
      children={
        <>
          <ConfirmPasswordModal
            reason="Please confirm your password to edit your account details"
            onSuccess={confirmPasswordSuccess}
            onCancel={closeConfirmPasswordModal}
            onFailure={confirmPasswordFailed}
            opened={confirmPasswordModalOpened}
          />

          <Stack w="100%" gap={"xs"}>
            <form
              id="update-account"
              style={{ width: "100%" }}
              onSubmit={form.onSubmit(
                async (data) => await updateAccount.mutateAsync(data),
              )}
            >
              <Stack w="100%" gap={"xs"}>
                <FieldWrapper
                  name="Username"
                  direction={{ base: "column", sm: "row" }}
                  loading={loading}
                >
                  <TextInput
                    flex="1 1 0"
                    {...form.getInputProps("username")}
                    disabled={!editing}
                    ta={"left"}
                  />
                </FieldWrapper>
                <FieldWrapper
                  name="Email"
                  direction={{ base: "column", sm: "row" }}
                  loading={loading}
                >
                  <TextInput
                    flex="1 1 0"
                    {...form.getInputProps("email")}
                    disabled={!editing}
                    ta={"left"}
                    rightSection={(() => {
                      if (!accountQuery.data?.email) return;
                      return accountQuery.data?.emailConfirmed ? (
                        <IconCheck />
                      ) : (
                        <IconExclamationMark />
                      );
                    })()}
                  />
                </FieldWrapper>
                <FieldWrapper
                  name="Phone Number"
                  direction={{ base: "column", sm: "row" }}
                  loading={loading}
                >
                  <TextInput
                    flex="1 1 0"
                    {...form.getInputProps("phoneNumber")}
                    disabled={!editing}
                    ta={"left"}
                    rightSection={(() => {
                      if (!accountQuery.data?.phoneNumber) return;
                      return accountQuery.data?.phoneNumberConfirmed ? (
                        <IconCheck />
                      ) : (
                        <IconExclamationMark />
                      );
                    })()}
                  />
                </FieldWrapper>
                <EditButton
                  loading={loading}
                  editing={editing}
                  cancelEditDisabled={updateAccount.isPending}
                  onCancelEditClicked={() => {
                    form.reset();
                    setEditing(false);
                  }}
                  onEditClicked={() => setEditing(true)}
                  submitEditDisabled={!form.isDirty()}
                />
              </Stack>
            </form>
            <Divider />
            <FieldWrapper
              name="Password"
              direction={{ base: "column", sm: "row" }}
              loading={loading}
            >
              <ChangePassword />
            </FieldWrapper>
          </Stack>
        </>
      }
      footer={<DangerSection />}
    />
  );
}
