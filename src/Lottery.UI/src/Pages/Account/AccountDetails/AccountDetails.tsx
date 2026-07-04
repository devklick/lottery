import {
  Divider,
  Flex,
  Stack,
  Text,
  TextInput,
  useMantineTheme,
} from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";
import { notifications } from "@mantine/notifications";
import {
  IconCheck,
  IconCircleCheck,
  IconExclamationMark,
  IconInfoCircle,
  IconXboxX,
} from "@tabler/icons-react";
import { useEffect, useState } from "react";

import ChangePassword from "./ChangePassword";
import DangerSection from "./DangerSection/DangerSection";
import EditButton from "./EditButton";
import {
  UpdateAccountRequestBody,
  updateAccountRequestBodySchema,
  UpdateAccountResponse,
} from "./schema";
import PageSection from "../../../components/PageSection";
import { useGetAccount, useUpdateAccount } from "../account.hooks";
import ConfirmPasswordModal from "../ConfirmPasswordModal";

interface AccountDetailsProps {
  authenticated: boolean;
}

export default function AccountDetails({ authenticated }: AccountDetailsProps) {
  const [editing, setEditing] = useState(false);

  const accountQuery = useGetAccount({ enabled: authenticated });

  const form = useForm<UpdateAccountRequestBody>({
    validate: schemaResolver(updateAccountRequestBodySchema),
    validateInputOnChange: true,
  });

  useEffect(() => {
    if (accountQuery.status === "success" && accountQuery.data) {
      form.setInitialValues({
        email: accountQuery.data.email ?? "",
        phoneNumber: accountQuery.data.phoneNumber ?? "",
        username: accountQuery.data.username ?? "",
      });
      form.reset();
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [accountQuery.data, accountQuery.status]);

  const { colors } = useMantineTheme();

  const [
    confirmPasswordModalOpened,
    { open: openConfirmPasswordModal, close: closeConfirmPasswordModal },
  ] = useDisclosure(false);

  const updateAccount = useUpdateAccount({
    onSuccess: updateAccountSuccess,
    onReAuthRequired: openConfirmPasswordModal,
  });

  async function updateAccountSuccess(data: UpdateAccountResponse) {
    notifications.show({
      title: "Account details updated",
      message: "Your new details have been saved",
      icon: <IconCircleCheck />,
      color: colors.green[5],
      autoClose: 5000,
    });
    if (data.email.messages?.[0].code === "EmailVerificationRequired") {
      notifications.show({
        title: "Verification email send",
        message:
          "Please check your emails to confirm the new email address. Once confirmed, it will be updated",
        icon: <IconInfoCircle />,
        color: colors.blue[5],
        autoClose: 5000,
      });
    }

    closeConfirmPasswordModal();
    setEditing(false);
    await accountQuery.refetch();
  }

  async function confirmPasswordSuccess() {
    await updateAccount.mutateAsync(form.getValues());
  }

  function confirmPasswordFailed() {
    notifications.show({
      title: "Unable to update account details",
      message: "Password verification failed",
      icon: <IconXboxX />,
      color: colors.red[5],
      autoClose: 5000,
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
                <Flex
                  w="100%"
                  gap={"xs"}
                  direction={{ base: "column", sm: "row" }}
                >
                  <Text ta={"start"} flex="1 1 0">
                    Username
                  </Text>
                  <TextInput
                    flex="1 1 0"
                    {...form.getInputProps("username")}
                    disabled={!editing}
                    ta={"left"}
                  />
                </Flex>
                <Flex
                  w="100%"
                  gap={"xs"}
                  direction={{ base: "column", sm: "row" }}
                >
                  <Text ta={"start"} flex="1 1 0">
                    Email
                  </Text>
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
                </Flex>
                <Flex
                  w="100%"
                  gap={"xs"}
                  direction={{ base: "column", sm: "row" }}
                >
                  <Text ta={"start"} flex="1 1 0">
                    Phone Number
                  </Text>
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
                </Flex>
                <EditButton
                  editing={editing}
                  cancelEditDisabled={updateAccount.isPending}
                  onCancelEditClicked={() => {
                    form.reset();
                    setEditing(false);
                  }}
                  onEditClicked={() => setEditing(true)}
                  submitEditDisabled={!form.isDirty()}
                />
                <Divider />
              </Stack>
            </form>

            <Flex w="100%" gap={"xs"} direction={{ base: "column", sm: "row" }}>
              <Text ta={"start"} flex={"1 1 0"}>
                Password
              </Text>
              <ChangePassword />
            </Flex>
          </Stack>
        </>
      }
      footer={<DangerSection />}
    />
  );
}
