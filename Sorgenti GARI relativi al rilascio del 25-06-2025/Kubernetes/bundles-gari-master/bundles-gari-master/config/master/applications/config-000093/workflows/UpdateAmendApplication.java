package com.abacogroup.pua.workflow.proc;

import com.abacogroup.applicationworkflowsdk.environment.IAgriProcedureEnvironment;
import com.abacogroup.applicationworkflowsdk.v2.procedures.ApplicationProcedure;
import com.abacogroup.dropwizard.auth.sso2.User;
import com.abacogroup.workflow.sdk.beans.ProcessData;
import com.abacogroup.workflow.sdk.beans.WorkflowMessage;
import lombok.extern.slf4j.Slf4j;

@Slf4j
public class UpdateAmendApplication extends ApplicationProcedure {

	@Override
	protected void doExecute(ProcessData processData, IAgriProcedureEnvironment env) throws Exception {
		log.debug("[no-spreading-periods-workflow] UpdateAmendApplication started now");
		try {
			User user = (User) processData.getAuthContext().getPrincipal();
			Long amendedApplicationId = this.getApplicationsWorkflowParams().getAmendedApplicationId();
			if(amendedApplicationId == null){
				return;
			}
			env.getAmendmentsService().amendApplication(processData.getTenantId(), amendedApplicationId, user, processData);
		} catch (Exception e) {
			processData.getWorkflowMessages().add(new WorkflowMessage("RECTIFICATION_ERROR", "Errore nella rettifica della domanda",
					WorkflowMessage.Type.ERROR));
		}
	}



}
