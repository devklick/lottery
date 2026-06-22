import {
  Button,
  ButtonGroup,
  Grid,
  Group,
  Text,
  TextInput,
  Tooltip,
  useMantineTheme,
} from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";
import { useDisclosure, useMediaQuery } from "@mantine/hooks";
import { notifications } from "@mantine/notifications";
import {
  IconCheck,
  IconCircleCheck,
  IconInfoCircle,
  IconX,
  IconXboxX,
} from "@tabler/icons-react";
import { useEffect, useState } from "react";

import PageSection from "../../components/PageSection";
import { useGetAccount, useUpdateAccount } from "../account.hooks";
import {
  UpdateAccountRequestBody,
  updateAccountRequestBodySchema,
  UpdateAccountResponse,
} from "./updateAccountDetails.schema";
import ConfirmPasswordModal from "../ConfirmPasswordModal";

interface AccountDetailsProps {
  authenticated: boolean;
}

export default function AccountDetails({ authenticated }: AccountDetailsProps) {
  const [editing, setEditing] = useState(false);

  const accountQuery = useGetAccount({ enabled: authenticated });

  const form = useForm<UpdateAccountRequestBody>({
    validate: schemaResolver(updateAccountRequestBodySchema),
    initialValues: accountQuery.data,
    validateInputOnChange: true,
  });

  // TODO: Fix this nonsense - it doesnt work.
  // After the form is submitted and the mutation is successful, we want to
  // refetch the account details and update them in the form.
  // This shouldnt be difficult...
  useEffect(() => {
    if (accountQuery.status === "success" && accountQuery.data) {
      form.setInitialValues(accountQuery.data);
      form.reset();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [accountQuery.data, accountQuery.status]);

  const { breakpoints, colors } = useMantineTheme();
  const sm = useMediaQuery(`(max-width: ${breakpoints.sm})`);
  const xs = useMediaQuery(`(max-width: ${breakpoints.xs})`);

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

    console.log(data);
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
      subheader="Here you can find the basic information account your account"
      collapsable
      children={
        <>
          {confirmPasswordModalOpened && (
            <ConfirmPasswordModal
              onSuccess={confirmPasswordSuccess}
              onCancel={closeConfirmPasswordModal}
              onFailure={confirmPasswordFailed}
            />
          )}

          <form
            id="update-account"
            onSubmit={form.onSubmit(
              async (data) => await updateAccount.mutateAsync(data),
            )}
          >
            <Grid w={"100%"} maw={600}>
              <Grid.Col span={5.5}>
                <Text ta={"right"}>Username</Text>
              </Grid.Col>
              <Grid.Col span={5.5}>
                <TextInput
                  {...form.getInputProps("username")}
                  disabled={!editing}
                  ta={"left"}
                  defaultValue={accountQuery.data?.username ?? ""}
                />
              </Grid.Col>
              <Grid.Col span={1} />

              <Grid.Col span={5.5}>
                <Text ta={"right"}>Email</Text>
              </Grid.Col>
              <Grid.Col span={5.5}>
                <TextInput
                  {...form.getInputProps("email")}
                  disabled={!editing}
                  ta={"left"}
                  defaultValue={accountQuery.data?.email ?? ""}
                />
              </Grid.Col>
              <Grid.Col span={1}>
                {accountQuery.data?.emailConfirmed ? (
                  <Tooltip label="Email confirmed">
                    <IconCheck size={16} />
                  </Tooltip>
                ) : (
                  <IconX size={16} />
                )}
              </Grid.Col>

              <Grid.Col span={5.5}>
                <Text ta={"right"}>Phone Number</Text>
              </Grid.Col>
              <Grid.Col span={5.5}>
                <TextInput
                  {...form.getInputProps("phoneNumber")}
                  disabled={!editing}
                  ta={"left"}
                  defaultValue={accountQuery.data?.phoneNumber ?? ""}
                />
              </Grid.Col>
              <Grid.Col span={1}>
                {accountQuery.data?.phoneNumber &&
                  (accountQuery.data?.phoneNumberConfirmed ? (
                    <Tooltip label="Phone number confirmed">
                      <IconCheck size={16} />
                    </Tooltip>
                  ) : (
                    <IconX size={16} />
                  ))}
              </Grid.Col>
            </Grid>
          </form>
        </>
      }
      footer={
        <Group justify="flex-end" w="100%">
          {!editing && <Button onClick={() => setEditing(true)}>Edit</Button>}
          {editing && (
            <ButtonGroup
              orientation={xs ? "vertical" : "horizontal"}
              w={sm ? "100%" : "auto"}
            >
              <Button
                disabled={updateAccount.isPending}
                type="reset"
                onClick={() => {
                  form.reset();
                  setEditing(false);
                }}
                fullWidth={sm}
                variant="outline"
                color="red"
              >
                Cancel
              </Button>
              <Button
                disabled={!form.isDirty()}
                type="submit"
                variant="gradient"
                gradient={{ from: "blue", to: "grape", deg: 20 }}
                fullWidth={sm}
                form={"update-account"}
              >
                Submit
              </Button>
            </ButtonGroup>
          )}
        </Group>
      }
    />
  );
}
